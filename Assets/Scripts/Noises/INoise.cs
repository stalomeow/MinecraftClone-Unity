namespace Minecraft.Noises
{
    public interface INoise
    {
        float Get(float x, float y, int octave, float persistence);

        float Get(float x, float y, float z, int octave, float persistence);
    }
}