using System;
using System.IO;
using System.Text.RegularExpressions;

string themesPath = @"c:\Users\viole\Documenti Locali\WPF\SamungUi.Demo\SamsungUi.Demo\SamsungUi\Themes";
var files = Directory.GetFiles(themesPath, "*.xaml");

int changedFiles = 0;

foreach (var file in files)
{
    string content = File.ReadAllText(file);
    string originalContent = content;
    
    // Rimpiazza i FontSize="XX" hardcoded
    content = Regex.Replace(content, @"FontSize=""(10|11)""", "FontSize=\"{DynamicResource OneUiExtraSmallFontSize}\"");
    content = Regex.Replace(content, @"FontSize=""(12|13)""", "FontSize=\"{DynamicResource OneUiSmallFontSize}\"");
    content = Regex.Replace(content, @"FontSize=""(14)""", "FontSize=\"{DynamicResource OneUiNormalFontSize}\"");
    content = Regex.Replace(content, @"FontSize=""(15|16|17|18)""", "FontSize=\"{DynamicResource OneUiSubtitleFontSize}\"");
    content = Regex.Replace(content, @"FontSize=""(19|20|21|22|24)""", "FontSize=\"{DynamicResource OneUiTitleFontSize}\"");
    content = Regex.Replace(content, @"FontSize=""(25|26|27|28|30|32|34|36)""", "FontSize=\"{DynamicResource OneUiPageTitleFontSize}\"");

    if (content != originalContent)
    {
        File.WriteAllText(file, content);
        Console.WriteLine($"Normalized fonts in: {Path.GetFileName(file)}");
        changedFiles++;
    }
}

Console.WriteLine($"\nCompleted. Modified {changedFiles} files.");
