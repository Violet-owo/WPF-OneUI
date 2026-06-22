$themesPath = "c:\Users\viole\Documenti Locali\WPF\SamungUi.Demo\SamsungUi.Demo\SamsungUi\Themes"
$files = Get-ChildItem -Path $themesPath -Filter "*.xaml"

$changedFiles = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content

    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(10|11)"', 'FontSize="{DynamicResource OneUiExtraSmallFontSize}"')
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(12|13)"', 'FontSize="{DynamicResource OneUiSmallFontSize}"')
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(14)"', 'FontSize="{DynamicResource OneUiNormalFontSize}"')
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(15|16|17|18)"', 'FontSize="{DynamicResource OneUiSubtitleFontSize}"')
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(19|20|21|22|24)"', 'FontSize="{DynamicResource OneUiTitleFontSize}"')
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, 'FontSize="(25|26|27|28|30|32|34|36)"', 'FontSize="{DynamicResource OneUiPageTitleFontSize}"')

    if ($content -cne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Normalized fonts in: $($file.Name)"
        $changedFiles++
    }
}

Write-Host "`nCompleted. Modified $changedFiles files."
