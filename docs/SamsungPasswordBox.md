# SamsungPasswordBox

Il `SamsungPasswordBox` è il campo specifico per l'inserimento di dati sensibili (come password o PIN), progettato esteticamente come un `SamsungTextBox` ma con funzionalità di mascheramento dei caratteri e un pulsante integrato per rivelare temporaneamente la password.

![SamsungPasswordBox Example](../Screen/SamsungPasswordBox.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungPasswordBox` is the specific field for entering sensitive data (like passwords or PINs). It is aesthetically designed like a `SamsungTextBox` but includes character masking features and an integrated toggle button to reveal the password.

### Inheritance
This control inherits from `System.Windows.Controls.Control`. Internally, it manages a native `PasswordBox` and a `TextBox` to safely swap between masked and unmasked text.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **Password** | `string` | `""` | The actual text of the password. Unlike the native PasswordBox, this exposes a bindable DependencyProperty (Two-Way). |
| **IsPasswordRevealed** | `bool` | `False` | Controls the visibility of the characters. When `True`, the password is displayed in plain text. |

### Visual Behavior
- **Reveal Button**: An eye icon is displayed on the right edge of the text field. Clicking it toggles `IsPasswordRevealed`, swapping the masked view with the clear text view.
- **Design**: Shares the exact same rounded corners, surface background, and focus highlight border as the standard `SamsungTextBox`.

### How to Use
```xml
<sui:SamsungPasswordBox Password="{Binding UserPassword, Mode=TwoWay}" Width="250" />
```

---

## 🇮🇹 Italiano

Il `SamsungPasswordBox` è il campo specifico per l'inserimento di dati sensibili (come password o PIN), progettato esteticamente come un `SamsungTextBox` ma con funzionalità di mascheramento dei caratteri e un pulsante integrato per rivelare temporaneamente la password.

### Ereditarietà
Questo controllo eredita da `System.Windows.Controls.Control`. Internamente, gestisce un `PasswordBox` nativo e un `TextBox` base, permettendo lo scambio sicuro tra il testo mascherato e quello in chiaro.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Password** | `string` | `""` | Il testo effettivo della password. A differenza del PasswordBox nativo, questo espone una DependencyProperty bindabile (Bidirezionale). |
| **IsPasswordRevealed** | `bool` | `False` | Controlla la visibilità dei caratteri. Quando è a `True`, la password viene mostrata in chiaro. |

### Comportamento Visivo
- **Pulsante Rivela**: Sul margine destro compare l'icona di un occhio. Cliccandola si inverte il valore di `IsPasswordRevealed`, scambiando la vista mascherata (con i puntini neri) con quella in chiaro.
- **Design Base**: Condivide gli stessi bordi smussati, lo sfondo neutro di superficie e l'evidenziazione del bordo al focus del `SamsungTextBox` standard.

### Come Usarlo
```xml
<sui:SamsungPasswordBox Password="{Binding UserPassword, Mode=TwoWay}" Width="250" />
```
