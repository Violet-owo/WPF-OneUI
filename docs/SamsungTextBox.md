# SamsungTextBox

Il `SamsungTextBox` è una versione stilizzata e moderna della classica casella di testo WPF. Aggiunge bordi arrotondati e una speciale modalità "Barra di Ricerca".

![SamsungTextBox Example](../Screen/SamsungTextBox.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungTextBox` is a stylized, modern version of the classic WPF text box. It adds rounded borders, a soft surface background, and a special "Search Bar" mode.

### Inheritance
Inherits directly from `System.Windows.Controls.TextBox`. You can use all standard properties like `Text`, `MaxLength`, `CaretBrush`, etc.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **IsSearchBar** | `bool` | `False` | When set to `True`, a magnifying glass icon appears inside the text box, giving it the classic look of a search bar. |

### Visual Behavior
- **Default**: A slightly rounded rectangle filled with a neutral surface color.
- **Focused**: The border gently highlights with the primary accent color.
- **IsSearchBar=True**: Adds a built-in search icon on the left padding.

### How to Use
```xml
<!-- Standard text field -->
<sui:SamsungTextBox Text="Hello World" />

<!-- Search bar style -->
<sui:SamsungTextBox IsSearchBar="True" Width="250" />
```

---

## 🇮🇹 Italiano

Il `SamsungTextBox` è una versione stilizzata e moderna della classica casella di testo WPF. Aggiunge bordi arrotondati, uno sfondo neutro "di superficie" e una speciale modalità "Barra di Ricerca".

### Ereditarietà
Eredita direttamente da `System.Windows.Controls.TextBox`. Puoi usare tutte le proprietà standard come `Text`, `MaxLength`, `CaretBrush`, ecc.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **IsSearchBar** | `bool` | `False` | Se impostata a `True`, appare l'icona di una lente d'ingrandimento all'interno della casella di testo, trasformandola graficamente in una barra di ricerca. |

### Comportamento Visivo
- **Default**: Un rettangolo leggermente smussato, riempito con il colore di superficie.
- **Focused**: Il bordo si evidenzia dolcemente usando il colore di accento principale.
- **IsSearchBar=True**: Aggiunge un'icona di ricerca integrata sul lato sinistro.

### Come Usarlo
```xml
<!-- Campo di testo standard -->
<sui:SamsungTextBox Text="Ciao Mondo" />

<!-- Stile barra di ricerca -->
<sui:SamsungTextBox IsSearchBar="True" Width="250" />
```
