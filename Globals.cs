using System.Collections.Generic;

public class Globals
{
    public static Dictionary<string, bool> HIGHER_FUNCTIONS = new Dictionary<string, bool>()
    {
        ["Subliminal Messages"] = true,
        ["Insomnia"] = true,
        ["Anxiety"] = true,
        ["Night Terrors"] = true,
        ["Sleep Paralysis"] = true,
    };

    public static bool IsHigherFunctionEnabled(string higherFunction)
    {
        return HIGHER_FUNCTIONS[higherFunction];
    }
}