/*
    Copyright (c) 2019 SaladBadger
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Descent2Workshop.Editor.Renderer
{
    public class Camera
    {
        //Orientation is used to orbit around position
        private Matrix4 orientation;
        //The base point for orbiting. 
        private Vector3 position;
        //Distance of the camera from the base point. 
        private float distance;
        //Projection matrix
        private Matrix4 projection;

        private float angle = 0, pitch = 0;

        public Camera()
        {
            position = new Vector3(0, 0, 0);
            orientation = Matrix4.CreateRotationX(0.0f);
            distance = 50.0f;
        }

        public void MakeOrthoCamera(float scale, float aspect)
        {
            if (aspect < 1.0f)
                projection = Matrix4.CreateOrthographic(scale, scale * aspect, -1200f, 1200f);
            else
                projection = Matrix4.CreateOrthographic(scale * aspect, scale, -1200f, 1200f);
        }

        public void MakePerpectiveCamera(float fov, float aspect)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, 0.1f, 1500.0f);
        }

        public void SetShaderProjection(Shader shader)
        {
            shader.UseShader();
            int projectMatrix = GL.GetUniformLocation(shader.shaderID, "projectMatrix");
            GL.UniformMatrix4(projectMatrix, false, ref projection);
            GLUtilities.ErrorCheck("Setting shader projection");

            int cameraPos = GL.GetUniformLocation(shader.shaderID, "cameraPos");
            int cameraOrientation = GL.GetUniformLocation(shader.shaderID, "cameraOrientation");
            int cameraDistance = GL.GetUniformLocation(shader.shaderID, "cameraDistance");

            GL.Uniform3(cameraPos, ref position);
            GL.UniformMatrix4(cameraOrientation, false, ref orientation);
            GL.Uniform1(cameraDistance, distance);
            GLUtilities.ErrorCheck("Setting shader orientation");
        }

        public void Translate(Vector3 amount)
        {
            Vector4 hAmount = new Vector4(amount, 0.0f);
            hAmount = Vector4.Transform(orientation, hAmount);
            position += hAmount.Xyz;
        }

        public void Rotate(Vector3 axis, float angle)
        {
            Matrix4 rotMatrix = Matrix4.CreateFromAxisAngle(axis, angle);
            //orientation *= rotMatrix;
            orientation = rotMatrix * orientation;
        }

        public void Rotate(float angle, float pitch)
        {
            //Matrix4 pitchMat = Matrix4.CreateRotationX(pitch);
            //Matrix4 angleMat = Matrix4.CreateRotationY(angle);
            //orientation = pitchMat * angleMat * orientation;
            this.angle += angle;
            this.pitch += pitch;
            Matrix4 pitchMat = Matrix4.CreateRotationX(this.pitch);
            Matrix4 angleMat = Matrix4.CreateRotationY(this.angle);
            orientation = angleMat * pitchMat;
        }

        public void GetUpSide(out Vector3 up, out Vector3 side)
        {
            up = orientation.Column1.Xyz;
            side = orientation.Column0.Xyz;
        }

        public void Pitch(float angle)
        {
            Vector3 rot = new Vector3(orientation.M11, orientation.M21, orientation.M31);
            Rotate(rot, angle);
        }

        public bool CameraFacingPoint(Vector3 point)
        {
            Vector3 localPoint = new Vector3(-point.X, point.Y, point.Z);
            Vector4 cameraPosition = new Vector4(localPoint + position, 1.0f);
            cameraPosition = Vector4.Transform(cameraPosition, orientation);
            cameraPosition += new Vector4(0.0f, 0.0f, -distance, 0.0f);
            Vector4 diff = Vector4.Transform(cameraPosition, projection);
            //Console.WriteLine(facing);
            //diff.Normalize();
            //float dot = Vector3.Dot(diff, facing);
            //return dot > 0.0f;

            return diff.Z >= 0.0f;
        }

        public Vector4 TransformPoint(Vector3 point)
        {
            Vector3 localPoint = new Vector3(-point.X, point.Y, point.Z);
            Vector4 cameraPosition = new Vector4(localPoint + position, 1.0f);
            cameraPosition = Vector4.Transform(cameraPosition, orientation);
            cameraPosition += new Vector4(0.0f, 0.0f, -distance, 0.0f);
            cameraPosition = Vector4.Transform(cameraPosition, projection);
            return cameraPosition;
        }

        public void UpdateShader(Shader shader)
        {
            shader.UseShader();
            int cameraPos = GL.GetUniformLocation(shader.shaderID, "cameraPos");
            int cameraOrientation = GL.GetUniformLocation(shader.shaderID, "cameraOrientation");
            int cameraDistance = GL.GetUniformLocation(shader.shaderID, "cameraDistance");

            GL.Uniform3(cameraPos, ref position);
            GL.UniformMatrix4(cameraOrientation, false, ref orientation);
            GL.Uniform1(cameraDistance, distance);
            GLUtilities.ErrorCheck("Setting shader camera");
        }

        public void Dolly(float amount)
        {
            distance += amount;
            if (distance < 0) distance = 0;
        }
    }
}