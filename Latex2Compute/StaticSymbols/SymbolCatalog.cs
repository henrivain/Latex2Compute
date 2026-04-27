namespace Latex2Compute.StaticSymbols;

internal static class SymbolCatalog
{
    public static char[] MathOperators { get; } = ['/', '*', '-', '+', '(', ')', '[', ']', '{', '}'];

    public static Dictionary<string, string> PhysicsTranslations { get; } = new()
    {
        // Rise to power does not matter, because it is translated automatically

        // constants
        ["c"] = "_c",   // light speed
        ["u"] = "_u",   // atom mass unit

        // TODO: Add "prefer constants over units"- setting
        // ["h"] = "_h",   // planck constant   (already hour)
        // ["g"] = "_g",   // gravitational acceleration    (already gram)

        ["pm"] = "10^(-12)_m",
        ["nm"] = "_nm",
        ["mm"] = "_mm",
        ["cm"] = "_cm",
        ["dm"] = "_dm",
        ["m"] = "_m",
        ["dam"] = "10_m",
        ["hm"] = "10^(2)_m",
        ["km"] = "_km",
        ["pc"] = "_pc",
        ["kpc"] = "10^(3)_pc",
        ["ly"] = "_ltyr",

        ["ml"] = "_ml",
        ["cl"] = "10^(2)_l",
        ["dl"] = "10_l",
        ["l"] = "_l",

        ["mW"] = "10^(-3)_W",
        ["W"] = "_W",
        ["kW"] = "_kW",
        ["MW"] = "10^(6)_W",
        ["GW"] = "10^(9)_W",
        ["TW"] = "10^(12)_W",
        ["Wh"] = "10^(-3)_kWh",
        ["kWh"] = "_kWh",
        ["MWh"] = "10^(3)_kWh",
        ["GWh"] = "10^(6)_kWh",
        ["TWh"] = "10^(9)_kWh",
        ["PWh"] = "10^(12)_kWh",

        ["kA"] = "_kA",
        ["A"] = "_A",
        ["mA"] = "_mA",

        ["kV"] = "_kV",
        ["V"] = "_V",
        ["mV"] = "_mV",

        ["mT"] = "10^(-3)_T",
        ["T"] = "_T",

        ["nC"] = "10^(-9)_coul",
        ["\\muC"] = "10^(-6)_coul",
        ["mC"] = "10^(-3)_coul",
        ["C"] = "_coul",
        ["G"] = "_Gs",

        ["F"] = "_F",
        ["nF"] = "_nF",
        ["pF"] = "_pF",
        ["Wb"] = "_Wb",

        ["mN"] = "10^(-3)_N",
        ["N"] = "_N",
        ["kN"] = "10^(3)_N",
        ["MN"] = "10^(6)_N",
        ["GN"] = "10^(9)_N",

        ["mg"] = "_mg",
        ["g"] = "_gm",
        ["kg"] = "_kg",

        ["ns"] = "_ns",
        ["ms"] = "_ms",
        ["s"] = "_s",
        ["\\min"] = "_min",
        ["h"] = "_hr",
        ["d"] = "_day",
        ["a"] = "_yr",

        ["mol"] = "_mol",
        ["mmol"] = "10^(-3)_mol",

        ["cd"] = "_cd",

        ["r"] = "",
        ["rad"] = "",
        ["(rad)"] = "",
        ["\\degree"] = "",
        ["°"] = "",

        ["eV"] = "_eV",
        ["keV"] = "10^(3)_eV",
        ["MeV"] = "10^(6)_eV",
        ["GeV"] = "10^(9)_eV",
        ["TeV"] = "10^(12)_eV",

        ["J"] = "_J",
        ["kJ"] = "10^(3)_J",
        ["MJ"] = "10^(6)_J",
        ["GJ"] = "10^(9)_J",
        ["TJ"] = "10^(12)_J",


        ["K"] = "_°K",
        ["°C"] = "_°C",
        ["°F"] = "_°F",

        ["cal"] = "_cal",
        ["kcal"] = "_kcal",

        ["Hz"] = "_Hz",
        ["kHz"] = "_kHz",
        ["MHz"] = "_MHz",
        ["GHz"] = "_GHz",


        ["mbar"] = "_mbar",
        ["bar"] = "_bar",
        ["kbar"] = "10^(3)_bar",
        ["Pa"] = "_Pa",
        ["kPa"] = "_kPa",
        ["MPa"] = "10^(6)_Pa",
        ["GPa"] = "10^(9)_Pa",

        [@"m\Omega"] = "10^(-3)_Ω",
        [@"\Omega"] = "_ohm",
        [@"k\Omega"] = "_kΩ",
        [@"M\Omega"] = "_MΩ",

        ["Bq"] = "(1/_s)",
        ["kBq"] = "10^(3)*(1/_s)",
        ["MBq"] = "10^(6)*(1/_s)",
        ["GBq"] = "10^(9)*(1/_s)",
    };

    public static string[] PhysicsEraseable = [
            "mm^3", "cm^3", "dm^3", "nm^3", "km^3", "m^3",
            "mm^2", "cm^2", "dm^2", "nm^2", "km^2", "m^2",
            "keV", "MeV", "GeV", "TeV", "eV",
            "MHz", "kHz", "Hz",
            "kcal", "cal",
            "MWh", "GWh", "TWh", "kWh", "PWh", "kW",
            "mmol", "mol",
            "kpl",
            "\\min", "\\max",
            "kPa", "bar", "Pa",
            "dam", "cm", "dm", "nm", "pm", "km", "hm", "mm",
            "lx", "lm", "Wb", "sr",
            "kN",
            "kJ", "MJ", "GJ",
            "kg", "mg",
            "°F", "°C",
            "cd",
            "kA", "mA",
            "kV", "mV",
            "ms", "ns",
            "ml", "cl", "dl",
            "A","C", "F", "g", "G", "h", "J", "K", "l",
            "m", "N", "r", "s", "T", "V", "W",
        ];

    public static string[] GreekSymbols =
        [
            "k\\Omega", "\\Omega", "\\Gamma", "\\Delta",
            "\\varepsilon", "\\zeta", "\\eta", "\\theta",
            "\\vartheta", "\\iota", "\\kappa", "\\lambda",
            "\\Lambda", "\\mu", "\\nu", "\\xi", "\\Xi", "\\Pi",
            "\\rho", "\\sigma", "\\Sigma", "\\tau", "\\upsilon",
            "\\Upsilon", "\\phi", "\\Phi", "\\chi", "\\psi",
            "\\Psi", "\\omega", "\\partial", "\\varphi"
        ];
}
