using System.Net.Mime;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

namespace Services;

public class ThemeService
{
    private static ThemeService? _instance;
    public static ThemeService Instance => _instance ??= new ThemeService();
    
    private const string LightThemePath = "avares://UI/Styles/Themes/Light.axaml";
    private const string DarkThemePath = "avares://UI/Styles/Themes/Dark.axaml";

    public bool IsDarkTheme { get; private set; }
    
    private const string SettingsFile = "themeSettings.txt";
    private static readonly string SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFile);

    /// <summary>
    /// Toggles the theme from light to dark and vice versa
    /// </summary>
    public void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        ApplyTheme(IsDarkTheme ? DarkThemePath : LightThemePath);
        SaveThemeSettings();
    }

    /// <summary>
    /// Sets the theme to the given theme path
    /// </summary>
    /// <param name="themePath"></param>
    private void ApplyTheme(string themePath)
    {
        if (Application.Current is not { } app) return;

        try
        {
            var newTheme = new StyleInclude(new Uri("avares://UI/"))
            {
                Source = new Uri(themePath)
            };

            // Überprüfen, ob ein Theme existiert und ersetzen
            var existingTheme = app.Styles.OfType<StyleInclude>()
                .FirstOrDefault(s => s.Source?.ToString().Contains("Themes/") == true);

            if (existingTheme != null)
            {
                app.Styles.Remove(existingTheme);
            }

            app.Styles.Add(newTheme);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Laden des Themes: {ex.Message}");
        }
    }
    /// <summary>
    /// Saves the currently selected theme to a file where 0 = LightTheme and 1 = DarkTheme
    /// </summary>
    private void SaveThemeSettings()
    {
        File.WriteAllText(SettingsFilePath, IsDarkTheme ? "1" : "0");
    }

    /// <summary>
    /// Reads the saved theme settings file and sets the theme accordingly
    /// </summary>
    public void LoadThemeSettings()
    {
        if (!File.Exists(SettingsFilePath)) return;
        
        var settings = File.ReadAllText(SettingsFilePath);
        IsDarkTheme = settings.Equals("1");
        ApplyTheme(IsDarkTheme ? DarkThemePath : LightThemePath);
    }
}