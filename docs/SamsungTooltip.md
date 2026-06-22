# SamsungTooltip

Il `SamsungTooltip` non è un controllo C# separato, ma uno stile WPF personalizzato che sovrascrive il noioso riquadro giallo rettangolare di sistema con un elegante fumetto nero arrotondato, in puro stile One UI.

![SamsungTooltip Example](../Screen/SamsungTooltip.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungTooltip` is not a separate C# control, but rather a custom WPF Style that overrides the boring, rectangular, system-yellow tooltip box with an elegant, rounded, dark speech bubble in pure One UI style.

### Inheritance
This is a standard `System.Windows.Controls.ToolTip` styled natively across the application via `TargetType="ToolTip"`.

### Visual Behavior
- **Design**: Pill-shaped or heavily rounded rectangle with a dark, semi-transparent background (`#CC000000`) and white text.
- **Shadow**: Subtle drop shadow (`DropShadowEffect`) to give it depth.
- **Global Scope**: Once `SamsungUi.Controls.xaml` is merged in `App.xaml`, EVERY tooltip in your entire application will automatically inherit this gorgeous style without changing a single line of code.

### How to Use
Just use the standard WPF `ToolTip` property on any control!

```xml
<sui:SamsungButton Content="Hover Me" ToolTip="This is a gorgeous tooltip!" />
```

---

## 🇮🇹 Italiano

Il `SamsungTooltip` non è un nuovo controllo C#, ma uno Stile WPF globale (`TargetType="ToolTip"`) che sovrascrive il noioso riquadro rettangolare e ingiallito di sistema con un elegante fumetto nero arrotondato, in puro stile One UI.

### Ereditarietà
Si tratta di un semplice `System.Windows.Controls.ToolTip` stilizzato nativamente a livello di applicazione.

### Comportamento Visivo
- **Design**: Una forma a pillola (o un rettangolo ampiamente smussato) con uno sfondo scuro e semi-trasparente (`#CC000000`) e testo bianco nitido.
- **Ombra**: Ha un `DropShadowEffect` per farlo sembrare sospeso sopra l'interfaccia.
- **Portata Globale**: Essendo uno stile implicito, dal momento in cui includi le librerie `SamsungUi` in `App.xaml`, OGNI singolo ToolTip della tua applicazione adotterà automaticamente questa veste grafica senza che tu debba scrivere una riga di codice in più.

### Come Usarlo
Usa semplicemente la proprietà standard `ToolTip` di WPF su qualsiasi controllo!

```xml
<sui:SamsungButton Content="Passa il mouse" ToolTip="Questo è un fantastico tooltip!" />
```
