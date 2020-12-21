using UnityEngine;

[System.Serializable]
public class FastNoiseSettings
{
    public AnimationCurve levels = AnimationCurve.Linear(0, 1, 0, 1);
    public FastNoise.NoiseType noiseType;
    public int seed;
    public float frequency;

    public FastNoise.FractalType fractalType;
    public int octaves;
    public float gain;
    public float lacunarity;

    private FastNoise fs = new FastNoise();

    public void SetParams()
	{
        int s = seed;

        if(seed == -1)
		{
            s = Random.Range(int.MinValue, int.MaxValue);
		}

        fs.SetNoiseType(noiseType);
        fs.SetSeed(s);
        fs.SetFrequency(frequency);
        fs.SetFractalType(fractalType);
        fs.SetFractalGain(gain);
        fs.SetFractalLacunarity(lacunarity);
	}

    public float Get(float x, float y)
	{
        return levels.Evaluate(0.5f + fs.GetNoise(x, y)/2f);
	}
}
