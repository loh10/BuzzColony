public class SeedConverter
{
    /// <summary>
    /// Convert string into int seed
    /// </summary>
    /// <returns>seed int</returns>
    public int SeedConvertion(string seedString)
    {
        int seed = 0;
        foreach (char caracter in seedString)
        {
            seed += caracter;
        }

        return seed;
    }
}
