$components = @(
    "SamsungCard", "SamsungCheckBox", "SamsungEditBox", "SamsungPasswordBox", 
    "SamsungTextBox", "SamsungListBox", "SamsungSlider", "SamsungRadioButton", 
    "SamsungProgressBar", "SamsungStopwatch", "SamsungTabControl", "SamsungToggleSwitch", 
    "SamsungExpandablePage", "SamsungSegmentedBar", "SamsungModal", "SamsungNavigationBar", 
    "SamsungToastService", "SamsungDateTimePicker", "SamsungColorPicker", "SamsungChart", 
    "SamsungComboBox", "SamsungBadge", "SamsungStepper", "SamsungExpander", 
    "SamsungTooltip", "SamsungWindow"
)

foreach ($c in $components) {
    $path = "docs\$c.md"
    if (-not (Test-Path $path)) {
        $content = "# $c`n`n![$c Example](../Screen/$c.png)`n> *Screenshot is on a coffee break! The developer will upload it shortly.*`n> *Lo screenshot e' in pausa caffe'! Lo sviluppatore lo carichera' a breve.*`n`n---`n`n## English`n`nThe `$c` is a core element of the **SamsungUi** library, designed to bring the fluid and rounded aesthetics of One UI to your WPF applications.`n`n### Inheritance`nThis control inherits from the standard WPF equivalent (or `Control`), fully supporting native properties, bindings, and events.`n`n### How to Use`n`n```xml`n<sui:$c />`n````n`n---`n`n## Italiano`n`nIl `$c` e' un elemento essenziale della libreria **SamsungUi**, progettato per portare l'estetica fluida e tondeggiante della One UI nelle tue applicazioni WPF.`n`n### Ereditarieta'`nQuesto controllo eredita dall'equivalente standard WPF (o da `Control`), supportando nativamente tutte le proprieta', i binding e gli eventi classici.`n`n### Come Usarlo`n`n```xml`n<sui:$c />`n````n"
        Set-Content -Path $path -Value $content -Encoding UTF8
    }
}
