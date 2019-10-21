namespace LibDescent.Data
{
    public class LevelTexture
    {
        public LevelTexture(ushort textureIndex)
        {
            TextureIndex = textureIndex;
        }

        public ushort TextureIndex { get; }
    }
}