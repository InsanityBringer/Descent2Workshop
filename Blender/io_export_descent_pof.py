bl_info = {
    "name": "Descent .POF",
    "author": "SaladBadger",
    "version": (1, 2, 0),
    "blender": (2, 80, 0),
    "location": "File > Export > Descent (.POF)",
    "description": "Exports meshes to Descent Parallax Object files",
    "warning": "",
    "wiki_url": "",
    "tracker_url": "",
    "category": "Import-Export"}

import struct
import bpy
import bmesh
import math
import os
import io

def getMin(vec1, vec2):
    result = [0, 0, 0]
    if vec1[0] < vec2[0]:
        result[0] = vec1[0]
    else:
        result[0] = vec2[0]
    if vec1[1] < vec2[1]:
        result[1] = vec1[1]
    else:
        result[1] = vec2[1]
    if vec1[2] < vec2[2]:
        result[2] = vec1[2]
    else:
        result[2] = vec2[2]
    return result

def getMax(vec1, vec2):
    result = [0, 0, 0]
    if vec1[0] > vec2[0]:
        result[0] = vec1[0]
    else:
        result[0] = vec2[0]
    if vec1[1] > vec2[1]:
        result[1] = vec1[1]
    else:
        result[1] = vec2[1]
    if vec1[2] > vec2[2]:
        result[2] = vec1[2]
    else:
        result[2] = vec2[2]
    return result

def terribleNorm(vec):
    len = math.sqrt(vec[0] * vec[0] + vec[1] * vec[1] + vec[2] * vec[2])
    if len == 0:
        return [0.0, 0.0, 1.0];
    return [vec[0] / len, vec[1] / len, vec[2] / len]

def terribleLen(vec):
    return math.sqrt(vec[0] * vec[0] + vec[1] * vec[1] + vec[2] * vec[2])

def terribleAdd(vec1, vec2):
    return [vec1[0] + vec2[0], vec1[1] + vec2[1], vec1[2] + vec2[2]]

def writePadBytes(file, amount):
    for i in range(amount):
        file.write(struct.pack("<B", 0))

def fl2fix(fl):
    intval = int(fl * 65536.0)
    return intval
    
def terriblewrap(ang):
    ang = ang + 32768
    ang = ang & 65535
    return ang - 32768
    
def fl2fixang(fl):
    fl = fl / 1.57079635
    intval = int(fl * 16384)
    #print(str.format("angle is {0}, wrap to {1}", intval, terriblewrap(intval)))
    return terriblewrap(intval)

class NoMaterialException(Exception):
    def __init__(self, meshName):
        self.name = meshName
        
class TooLargeException(Exception):
    def __init__(self, meshName, displacement):
        self.name = meshName
        self.size = displacement
        
class NotUnwrappedException(Exception):
    def __init__(self, meshName):
        self.name = meshName
        
class WrongTypeException(Exception):
    def __init__(self, objName, objType):
        self.name = objName
        self.type = objType
        
class DisplacementTooLargeException(Exception):
    def __init__(self, meshName):
        self.name = meshName

class Gun:
    pass

class TMAPPoly:
    
    def __init__(self):
        numpoints = 0
        self.verts = []
        self.uvs = []
        self.avg = []
        self.normal = []
        texid = 0
        self.glow = -1
    
    def pack(self, stream, firstpoint):
        if self.glow != -1:
            stream.write(struct.pack("<HH", 8, int(self.glow)))
        ax = fl2fix(self.avg[0])
        ay = fl2fix(self.avg[2])
        az = fl2fix(self.avg[1])
        nx = fl2fix(self.normal[0])
        ny = fl2fix(self.normal[2])
        nz = fl2fix(self.normal[1])
        stream.write(struct.pack("<HHiiiiiih", 3, self.numpoints, -ax, ay, -az, -nx, ny, -nz, self.texid))
        #for v in self.verts:
        if len(self.verts) == 3:
            print(self.verts)
        for i in range(0, len(self.verts)):
            v = self.verts[i]
            stream.write(struct.pack("<H", v+firstpoint))
        if self.numpoints % 2 == 0:
            stream.write(struct.pack("<H", 0))
        #for uv in self.uvs: #UVs with an additional component?
        if len(self.verts) == 3:
            print(self.uvs)
        for i in range(0, len(self.uvs)):
            uv = self.uvs[i]
            stream.write(struct.pack("<iii", fl2fix(uv[0]), -fl2fix(uv[1]), 0))
            
class FLATPoly:
    
    def __init__(self):
        self.verts = []
        self.avg = []
        self.normal = []
        self.numpoints = 0
        self.colordata = 0
        self.glow = -1
    
    def setcolor(self, r, g, b):
        ri = int(r * 31)
        gi = int(g * 31)
        bi = int(b * 31)
        self.colordata = ((ri & 31) << 10) + ((gi & 31) << 5) + (bi & 31)
        
    def pack(self, stream, firstpoint):
        if self.glow != -1:
            stream.write(struct.pack("<HH", 8, int(self.glow)))
        ax = fl2fix(self.avg[0])
        ay = fl2fix(self.avg[2])
        az = fl2fix(self.avg[1])
        nx = fl2fix(self.normal[0])
        ny = fl2fix(self.normal[2])
        nz = fl2fix(self.normal[1])
        stream.write(struct.pack("<HHiiiiiih", 2, self.numpoints, -ax, ay, -az, -nx, ny, -nz, self.colordata))
        for v in self.verts:
            stream.write(struct.pack("<H", v+firstpoint))
        if self.numpoints % 2 == 0:
            stream.write(struct.pack("<H", 0))
            
class Points:

    def __init__(self):
        self.verts = []
        self.numverts = 0
    
    def write(self, file):
        #print(str.format("points array is {0} long and numverts is {1} long", len(self.verts), self.numverts))
        data = struct.pack("<H", self.numverts)
        file.write(data)
        for v in self.verts:
            data = struct.pack("<iii", fl2fix(v[0]), fl2fix(v[2]), fl2fix(v[1]))
            file.write(data)
            
class Animation:
    
    def __init__(self):
        self.numangs = 0
        self.angs = []
    
    def write(self, file):
        for langs in self.angs:
            #print(str.format("writing angs {0} {1} {2}", langs[0], langs[1], langs[2]))
            data = struct.pack("<hhh", fl2fixang(langs[0]), fl2fixang(langs[1]), -fl2fixang(langs[2]))
            file.write(data)
            
class Subobject:
    faces = []
    glowfaces = []
    numfaces = 0
    facesflat = []
    glowfacesflat = []
    numfacesflat = 0
    origin = []
    textures = []
    numtextures = 0
    parent = -1
    hasbuilt = 0
    
    def __init__(self):
        self.faces = []
        self.facesflat = []
        self.points = Points()
        self.anim = Animation()
        self.origin = []
        self.textures = []
        self.mins = [32000.0, 32000.0, 32000.0]
        self.maxs = [-32000.0, -32000.0, -32000.0]
        self.rad = 0
        self.ptr = 0; #Later exported as the subobject pointer into the interpreter data
        
    def addTextures(self, list):
        for tex in self.textures:
            if tex not in list:
                list.append(tex)
                
    def generateIDTA(self, datastream, firstpoint):
        startoffset = datastream.tell()
        for subobj in self.children:
            #data = data.join(struct.pack("<H", 7)) #SOBJCALL
            print(subobj.origin)
            datastream.write(struct.pack("<HhiiiHH", 6, subobj.id, -fl2fix(subobj.origin[0]), fl2fix(subobj.origin[2]), -fl2fix(subobj.origin[1]), 0, 0))
        
        self.ptr = datastream.tell()
        #data.append(struct.pack("<H", 6)) #DEFPSTART
        datastream.write(struct.pack("<HHHH", 7, self.points.numverts, firstpoint, 0))
        for v in self.points.verts:
            datastream.write(struct.pack("<iii", -fl2fix(v[0]), fl2fix(v[2]), -fl2fix(v[1])))
        for face in self.faces:
            face.pack(datastream, firstpoint)
        for face in self.facesflat:
            face.pack(datastream, firstpoint)
        for face in self.glowfaces:
            face.pack(datastream, firstpoint)
        for face in self.glowfacesflat:
            face.pack(datastream, firstpoint)
        datastream.write(struct.pack("<H", 0)) #END instruction
        destination = datastream.tell()
        pointpos = firstpoint + len(self.points.verts)
        subobjNum = 0
        for subobj in self.children:
            pointpos = subobj.generateIDTA(datastream, pointpos);
            #Finalize the SOBJCALL
            displacement = destination - startoffset - (20 * subobjNum)
            #Pointer is relative to the start of the instruction, but returns to the end of it. weird
            if displacement < 32768:
                datastream.seek((startoffset + (20 * subobjNum)) + 16)
                #struct.pack_into("<h", data, 18 * subobjNum, destination - (18 * subobjNum))
                datastream.write(struct.pack("<h", displacement))
            else:
                #This is killed for now, since the proposed instruction 9 was never implemented in any port or editor. 
                #print("Generating wide sobjcall")
                #datastream.seek((startoffset + (20 * subobjNum)))
                #datastream.write(struct.pack("<h", 9)) #Write new instruction header
                #datastream.seek((startoffset + (20 * subobjNum)) + 16)
                #struct.pack_into("<h", data, 18 * subobjNum, destination - (18 * subobjNum))
                #datastream.write(struct.pack("<i", displacement))
                raise DisplacementTooLargeException(self.name)
            datastream.seek(0, io.SEEK_END)
            destination = datastream.tell()
            subobjNum += 1
            
        return pointpos
    
    def writeSOBJ(self, file, version):
        print(str.format("id {0} parent {1}", self.id, self.parent))
        print(str.format("{0} faces in submodel", self.numfaces))
        if version > 8:
            size = 72
        else:
            size = 48
        data = struct.pack("<IIhh", 0x4A424F53, size, self.id, self.parent)
        file.write(data) #Write header and basic data
        norm = terribleNorm(self.origin)
        data = struct.pack("<iii", -fl2fix(norm[0]), fl2fix(norm[2]), -fl2fix(norm[1]))
        file.write(data) #Write normal
        data = struct.pack("<iii", -fl2fix(self.origin[0]), fl2fix(self.origin[2]), -fl2fix(self.origin[1]))
        file.write(data) #Write point on the plane. It's just for now the offset, blegh
        file.write(data) #Write the origin
        if version > 8:
            data = struct.pack("<iii", -fl2fix(self.mins[0]), fl2fix(self.mins[2]), -fl2fix(self.mins[1]))
            file.write(data) #Write mins
            data = struct.pack("<iii", -fl2fix(self.maxs[0]), fl2fix(self.maxs[2]), -fl2fix(self.maxs[1]))
            file.write(data) #Write maxs
        
        data = struct.pack("<ii", fl2fix(self.rad), self.ptr)
        file.write(data) #Write radius and IDTA pointer
    
    def generateAngles(self):
        newangs = []
        hackangs = self.mesh.rotation_euler.to_matrix().to_euler('ZYX')
        newangs.append(hackangs[0])
        newangs.append(hackangs[1])
        newangs.append(hackangs[2])
        self.anim.angs.append(newangs)
        #print(str.format("angs {0} are {1} {2} {3}", self.anim.numangs, self.mesh.rotation_euler[0], self.mesh.rotation_euler[1], self.mesh.rotation_euler[2]))
        self.anim.numangs += 1
            
    def createFromObject(self, meshobj, polymodel):
        self.mesh = meshobj
        isUnwrapped = True
        uvLayer = meshobj.data.uv_layers.active
        #If not unwrapped, all UVs on texture mapped faces need to get bashed to (0, 0)
        #Sometimes the uv layer also has no data in it, so also bash it then
        if len(meshobj.data.uv_layers) == 0:
            isUnwrapped = False
        elif len(uvLayer.data) == 0:
            isUnwrapped = False
        #Counter the parent's offset, since Descent wants everything global
        self.origin = (meshobj.location + meshobj.matrix_parent_inverse.translation)
        #Map names to generated IDs, since some ids will be lost on color faces
        if len(meshobj.data.materials) <= 0:
            raise NoMaterialException(meshobj.name)
        for mat in meshobj.data.materials:
            if mat.use_nodes:
                if mat.name not in polymodel.textures:
                    polymodel.textures[mat.name] = len(polymodel.textures);
                    polymodel.texList.append(mat.name)
        #print(str.format("added {0} textures", self.numtextures))
        #print(self.textures)
        self.points.numverts = len(meshobj.data.vertices)
        for vert in meshobj.data.vertices:
            #While loading the points, keep track of the global mins and max points
            self.mins = getMin(vert.co, self.mins)
            self.maxs = getMax(vert.co, self.maxs)
            self.points.verts.append(vert.co)
            lenFromOrigin = terribleLen(vert.co)
            pointFromOrigin = terribleAdd(vert.co, self.origin)
            lenFromMainOrigin = terribleLen(pointFromOrigin)
            if lenFromOrigin > self.rad:
                self.rad = lenFromOrigin
            if lenFromMainOrigin > polymodel.radius:
                polymodel.radius = lenFromMainOrigin
        print(self.rad)
        for face in meshobj.data.polygons:
            #print(str.format("face has {0} verts", len(face.vertices)))
            mat = meshobj.data.materials[face.material_index]
            id = 0
            if mat.use_nodes:
                nface = TMAPPoly()
                #for v in face.vertices:
                for v in face.loop_indices:
                    nface.verts.append(meshobj.data.loops[v].vertex_index)
                    if isUnwrapped == False:
                        nface.uvs.append([0, 0])
                    else:
                        uvloop = uvLayer.data[v]
                        nface.uvs.append(uvloop.uv)
                    id += 1
                nface.avg = meshobj.data.vertices[face.vertices[0]].co
                nface.normal = face.normal
                nface.numpoints = len(face.vertices)
                #meh, i think it works though
                nface.texid = polymodel.textures[mat.name]
                glow = mat.get("glow")
                if glow != None:
                    nface.glow = glow;
                nface.verts.reverse()
                nface.uvs.reverse()
                if nface.glow != -1:
                    self.glowfaces.append(nface)
                else:
                    self.faces.append(nface)
                    self.numfaces += 1
            else:
                nface = FLATPoly()
                for v in face.vertices:
                    nface.verts.append(v)
                    id += 1
                #nface.avg = face.center
                nface.avg = meshobj.data.vertices[face.vertices[0]].co
                nface.normal = face.normal
                nface.numpoints = len(face.vertices)
                color = meshobj.data.materials[face.material_index].diffuse_color
                nface.setcolor(math.pow(color[0], .45454545), math.pow(color[1], .45454545), math.pow(color[2], .45454545))
                glow = mat.get("glow")
                if glow != None:
                    nface.glow = glow;
                nface.verts.reverse()
                if nface.glow != -1:
                    self.glowfacesflat.append(nface)
                else:
                    self.facesflat.append(nface)
                    self.numfacesflat += 1
                
#I don't know how to program in languages like python aaa
class Polymodel:
    def __init__(self):
        self.textures = dict()
        self.texList = []
        self.mins = [32000.0, 32000.0, 32000.0]
        self.maxs = [-32000.0, -32000.0, -32000.0]
        self.radius = 0
        self.subobjects = []
        self.gunpts = []
        self.numframes = 0;
        
    def buildPolymodel(self, startObj):
        basemodel = Subobject()
        basemesh = startObj #bpy.context.selected_objects[0]
        if basemesh.type == 'MESH':
            print("Generating subobject 0")
            basemodel.createFromObject(basemesh, self)
            print(basemodel.textures)
            self.subobjects.append(basemodel)
            basemodel.id = 0
            basemodel.children = []
            basemodel.name = basemesh.name
            curid = 0
            for subobj in basemesh.children:
                tuple = self.recursiveBuild(subobj, curid, 0, basemodel)
                curid = tuple[0]
                if tuple[1].id != -1:
                    basemodel.children.append(tuple[1])
            for subobj in self.subobjects:
                #This doesn't account for the entire offset chain, but this appears to be accurate to parallax's own tools
                self.mins = getMin(self.mins, [subobj.origin[0]+subobj.mins[0], subobj.origin[1]+subobj.mins[1], subobj.origin[2]+subobj.mins[2]])
                self.maxs = getMax(self.maxs, [subobj.origin[0]+subobj.maxs[0], subobj.origin[1]+subobj.maxs[1], subobj.origin[2]+subobj.maxs[2]])
                print(subobj.id)
        else:
            raise WrongTypeException(basemesh.name, basemesh.type)
            return
        
        print(self.textures)
        #We need a "texture list" to enable indexing names
        textureNames = self.textures.keys()
        print(self.texList)
        
        #Generate interpreter data
        idtaStream = io.BytesIO()
        self.subobjects[0].generateIDTA(idtaStream, 0)
        self.idta = idtaStream.getvalue()
    
        #Generate animations
        if bpy.context.scene.frame_start != bpy.context.scene.frame_end:
            for i in range(bpy.context.scene.frame_start, bpy.context.scene.frame_end+1):
                print(str.format("setting frame {0}", i))
                bpy.context.scene.frame_set(i)
                self.numframes += 1
                for subobj in self.subobjects:
                    subobj.generateAngles()
        
    def recursiveBuild(self, gameobj, curid, parentid, parent):
        model = Subobject()
        model.id = -1
        if gameobj.type == 'MESH':
            curid += 1
            print(str.format("adding recursive subobject {0}", curid))
            model.createFromObject(gameobj, self)
            model.children = []
            model.id = curid
            model.parent = parentid
            model.name = gameobj.name
            #parent.children.append(model)
            print(model.textures)
            self.subobjects.append(model)
            lparentid = curid
            for subobj in gameobj.children:
                tuple = self.recursiveBuild(subobj, curid, lparentid, model)
                curid = tuple[0]
                if tuple[1].id != -1:
                    model.children.append(tuple[1])
        elif gameobj.type == 'EMPTY':
            gunpt = Gun()
            gunpt.loc = (gameobj.location + gameobj.matrix_parent_inverse.translation)
            ang = gameobj.rotation_euler[2]
            pitch = gameobj.rotation_euler[0]
            gunpt.dir = [1 * math.cos(ang) * math.cos(pitch), 1 * math.sin(pitch), 1 * math.sin(ang) * math.cos(pitch)]
            gunpt.parent = parentid
            gunpt.id = len(self.gunpts)
            self.gunpts.append(gunpt)
        return (curid, model)
    
    def writeOHDR(self, file, version):
        data = struct.pack("<IIIiiiiiii", 0x5244484F, 32, len(self.subobjects), fl2fix(self.radius), -fl2fix(self.mins[0]), fl2fix(self.mins[2]), -fl2fix(self.mins[1]), fl2fix(-self.maxs[0]), fl2fix(self.maxs[2]), -fl2fix(self.maxs[1]))
        file.write(data)
        
    def writeANIM(self, file, version):
        length = len(self.subobjects) * self.numframes * 6 + 2
        padbytes = 0
        if version >= 8:
            padbytes = 4 - ((file.tell() + 8 + length) % 4)
            if padbytes == 4:
                padbytes = 0
            length += padbytes
        file.write(struct.pack("<IIH", 0x4D494E41, length, self.numframes))
        for subobj in self.subobjects:
            subobj.anim.write(file)
        writePadBytes(file, padbytes)
        
    def writeTXTR(self, file, version):
        textureNames = self.textures.keys()
        indicies = self.textures.values()
        texlen = 0
        for tex in textureNames:
            texlen += len(tex) + 1 #one character for null terminator
        texlen += 2 #number of textures
        padbytes = 0
        if version >= 8:
            padbytes = 4 - ((file.tell() + 8 + texlen) % 4)
            if padbytes == 4:
                padbytes = 0
            texlen += padbytes
        data = struct.pack("<IIH",0x52545854, texlen, len(textureNames)) #TXTR
        file.write(data)
        for tex in self.texList:
            file.write(tex.encode())
            data = struct.pack("<x")
            file.write(data)
        writePadBytes(file, padbytes)
        
    def writeGUNS(self, file, version):
        #0x534E5547
        length = (len(self.gunpts) * 28) + 4
        file.write(struct.pack("<III",0x534E5547, length, len(self.gunpts))) #GUNS
        for gun in self.gunpts:
            file.write(struct.pack("HH", gun.id, gun.parent))
            file.write(struct.pack("iii", -fl2fix(gun.loc[0]), fl2fix(gun.loc[2]), -fl2fix(gun.loc[1])))
            file.write(struct.pack("iii", -fl2fix(gun.dir[0]), fl2fix(gun.dir[2]), -fl2fix(gun.dir[1])))
            
    def writeIDTA(self, file):
        IDTAsize = len(self.idta)
        data = struct.pack("<II", 0x41544449, IDTAsize)
        file.write(data)
        file.write(self.idta)
    
    def writePOF(self, file, version):
        data = struct.pack("<IBB", 0x4F505350, version, 0)
        file.write(data)
        self.writeTXTR(file, version)
        self.writeOHDR(file, version)
        for subobj in self.subobjects:
            subobj.writeSOBJ(file, version)
        if len(self.gunpts) > 0:
            self.writeGUNS(file, version)
        if self.numframes > 0:
            self.writeANIM(file, version)
        self.writeIDTA(file)

from bpy.props import *
class DescentExporter(bpy.types.Operator):
    '''Export to Parallax Object File'''
    bl_idname = "export.pof"
    bl_label = 'Export .POF'
    filename_ext = ".pof"
             
    filepath: StringProperty(subtype = 'FILE_PATH')

    def execute(self, context):
        self.doPOFExport(self.properties.filepath)
        return {'FINISHED'}

    def invoke(self, context, event):
        wm = context.window_manager
        
        #Make sure the default filepath has a .pof extension
        #TODO: This should trim the .blend extension
        if self.filepath == "":
            self.filepath = bpy.context.blend_data.filepath
        
        self.filepath = bpy.path.ensure_ext(self.filepath, self.filename_ext)
        
        wm.fileselect_add(self)
        return {'RUNNING_MODAL'}

    @classmethod
    def poll(cls, context):
        return context.active_object != None
        
    def doPOFExport(self, filename):
        print("---STARTING---")
        #Is something selected?
        if len(bpy.context.selected_objects) < 1:
            return
        
        #kick the object out of edit mode since this is a bad idea
        if bpy.ops.object.mode_set.poll():
            bpy.ops.object.mode_set(mode='OBJECT')
                    
        polymodel = Polymodel()
        try:
            polymodel.buildPolymodel(bpy.context.selected_objects[0])
        except NoMaterialException as err:
            self.report({'ERROR'}, str.format("Mesh {0} does not have any defined materials.", err.name));
            return
        except WrongTypeException as err:
            self.report({'ERROR'}, str.format("Currently selected object {0} is a {1} and not a mesh.", err.name, err.type))
            return
        except NotUnwrappedException as err:
            self.report({'ERROR'}, str.format("Mesh {0} has textured faces, but no active UV map.", err.name))
            return
        except TooLargeException as err:
            self.report({'ERROR'}, str.format("Mesh {0}'s data size is greater than 32 kilobytes: {0}", err.name, err.displacement))
            return
        except DisplacementTooLargeException as err:
            self.report({'ERROR'}, str.format("Displacement exceeded 32768 bytes when generating object {0}'s children", err.name))
            return
        
        try:
            file = open(filename, "wb")
        except OSError as err:
            self.report({'ERROR'}, str.format("Error opening file {0}: {1}", err.filename, err.strerror))
            return
        if file != None:
            polymodel.writePOF(file, 8)
            file.close()
            
        if bpy.context.scene.frame_end - bpy.context.scene.frame_start + 1 > 5:
            self.report({'WARNING'}, "Scene contains more than 5 frames. This model won't work with Descent as-is.")
            
        if len(polymodel.subobjects) > 10:
            self.report({'WARNING'}, "Exported object contains more than 10 subobjects. This model won't work with Descent as-is.")
    
def exportIPOFOption(self, context):
    self.layout.operator_context = 'INVOKE_DEFAULT'
    self.layout.operator(DescentExporter.bl_idname, text="Descent .POF")
  
def register():
    bpy.utils.register_class(DescentExporter)
    bpy.types.TOPBAR_MT_file_export.append(exportIPOFOption)

def unregister():
    bpy.utils.unregister_class(DescentExporter)
    bpy.types.TOPBAR_MT_file_export.remove(exportIPOFOption)

if __name__ == "__main__":
    register()
