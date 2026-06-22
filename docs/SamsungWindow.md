# SamsungWindow

La `SamsungWindow` è una finestra personalizzata che rimuove il design di sistema rigido di Windows, offrendo bordi arrotondati e una barra del titolo coerente con l'estetica One UI (Chrome-less window).

![SamsungWindow Example](../Screen/SamsungWindow.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungWindow` is a custom, chrome-less window that strips away the rigid Windows OS framing, offering heavily rounded borders, customized drop shadows, and a title bar consistent with the One UI aesthetic.

### Inheritance
Inherits directly from `System.Windows.Window`. You can use it as the root `Window` element in your XAML.

### Custom Properties
There are no additional `DependencyProperty` configurations for this control. 
It overrides the native window chrome and attaches command bindings for Maximize, Minimize, Restore, and Close buttons automatically.

### Visual Behavior
- **Chrome-less**: Uses `WindowChrome` to remove the default OS title bar while retaining the ability to drag, resize, and snap to screen edges.
- **Title Bar**: Replaces the standard buttons with minimalist SVG/Path-based icons for Minimize, Maximize, and Close, featuring a subtle hover effect (especially a red hover for Close).
- **Rounding**: Enforces the signature `CornerRadius="20"` at the root window level.

### How to Use
Just change `<Window>` to `<sui:SamsungWindow>` in your main window XAML file!
```xml
<sui:SamsungWindow x:Class="MyApp.MainWindow"
                   xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                   xmlns:sui="clr-namespace:SamsungUi.Controls"
                   Title="MyApp" Height="600" Width="800">
    <Grid>
        <!-- App content here -->
    </Grid>
</sui:SamsungWindow>
```

---

## 🇮🇹 Italiano

La `SamsungWindow` è una finestra personalizzata (chrome-less) che rimuove i bordi squadrati e la barra di sistema di Windows, offrendo in cambio ampi bordi smussati, un'ombra morbida e una barra del titolo in puro stile One UI.

### Ereditarietà
Eredita da `System.Windows.Window`. Puoi tranquillamente sostituirla alla finestra base nel file `MainWindow.xaml` del tuo progetto.

### Proprietà Personalizzate
Questo controllo non introduce nuove `DependencyProperty`. Internamente si occupa di scavalcare la grafica nativa agganciando automaticamente i comandi di sistema per Ridurre a icona, Ingrandire e Chiudere.

### Comportamento Visivo
- **Senza Cornice (Chrome-less)**: Sfrutta `WindowChrome` per nascondere la barra del titolo dell'OS (ad esempio quella classica di Windows 10/11), pur mantenendo le funzionalità native come il trascinamento e l'ancoraggio (snapping) ai bordi dello schermo.
- **Pulsanti Finestra**: I pulsanti X, -, e [ ] sono stati completamente ridisegnati con percorsi vettoriali. Hanno un leggero effetto Hover integrato e quello di chiusura si colora elegantemente di rosso.
- **Arrotondamento Estremo**: Impone il classico `CornerRadius="20"` direttamente al contenitore radice della finestra per un look super moderno e "da tablet".

### Come Usarlo
Ti basta sostituire il tag principale `<Window>` con `<sui:SamsungWindow>` nel tuo file XAML principale:

```xml
<sui:SamsungWindow x:Class="MiaApp.MainWindow"
                   xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                   xmlns:sui="clr-namespace:SamsungUi.Controls"
                   Title="MiaApp" Height="600" Width="800">
    <Grid>
        <!-- Contenuto dell'applicazione -->
    </Grid>
</sui:SamsungWindow>
```
