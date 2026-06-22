# SamsungSegmentedBar

Il `SamsungSegmentedBar` è una barra di avanzamento multi-segmento orizzontale, perfetta per visualizzare la suddivisione percentuale dello spazio di archiviazione, budget, o qualsiasi metrica composta da più categorie.

![SamsungSegmentedBar Example](../Screen/SamsungSegmentedBar.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungSegmentedBar` is a horizontal multi-segment progress bar, perfect for visualizing the breakdown of storage space, budgets, or any metric composed of multiple categories.

### Inheritance
Inherits from `System.Windows.Controls.Control`. It generates its UI elements programmatically based on a collection of `ChartSegment` objects.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **Segments** | `IEnumerable<ChartSegment>`| `null` | The list of data points to render. Each `ChartSegment` has a Value, Label, and Brush. |
| **TotalValue** | `double` | `100` | The maximum value the bar represents. E.g., if total storage is 256GB, set this to 256. |
| **ValueUnit** | `string` | `""` | The suffix appended to numbers (e.g., "GB", "%", "$"). |
| **TitleText** | `string` | `""` | The main title text shown above the bar. |
| **DescriptionText**| `string` | `""` | Secondary text shown on the top right. |
| **SelectedSegment**| `ChartSegment` | `null` | The segment currently hovered/clicked by the user. |
| **IsPopupOpen** | `bool` | `False` | Opens a detail popup showing the `SelectedSegment` info. |

### Visual Behavior
- **Dynamic Widths**: The bar dynamically divides its width proportionally among all passed segments based on their `Value` relative to `TotalValue`.
- **Hover/Popup**: Hovering over a segment expands it slightly and triggers a sleek popup floating above the bar, detailing the exact value and label of that slice.
- **Empty Space**: If the sum of all segment values is less than `TotalValue`, the remaining space is rendered as a light gray "Free Space" bar.

### How to Use
```xml
<sui:SamsungSegmentedBar TitleText="System Storage" 
                         ValueUnit="GB" TotalValue="512" 
                         Segments="{Binding MyStorageData}" />
```

---

## 🇮🇹 Italiano

Il `SamsungSegmentedBar` è una barra di avanzamento multi-segmento orizzontale, perfetta per visualizzare la suddivisione percentuale dello spazio di archiviazione di un disco, un budget mensile, o qualsiasi metrica composta da più categorie.

### Ereditarietà
Eredita da `System.Windows.Controls.Control`. Genera programmaticamente i suoi elementi XAML (come i rettangoli arrotondati) basandosi sulla collezione di oggetti `ChartSegment` che gli viene passata.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Segments** | `IEnumerable<ChartSegment>`| `null` | La lista di dati da renderizzare. Ogni oggetto `ChartSegment` ha Valore, Etichetta e Colore. |
| **TotalValue** | `double` | `100` | Il valore massimo (il 100%) che la barra rappresenta. Es. in un disco da 256GB, va impostato a 256. |
| **ValueUnit** | `string` | `""` | L'unità di misura da accodare ai numeri (es. "GB", "%", "€"). |
| **TitleText** | `string` | `""` | Il titolo principale, mostrato in alto a sinistra. |
| **DescriptionText**| `string` | `""` | Testo secondario, mostrato in alto a destra. |
| **SelectedSegment**| `ChartSegment` | `null` | Il segmento attualmente sotto il cursore del mouse. |
| **IsPopupOpen** | `bool` | `False` | Apre il pannellino (Popup) fluttuante per mostrare i dettagli del segmento selezionato. |

### Comportamento Visivo
- **Larghezze Dinamiche**: La barra calcola la larghezza percentuale in tempo reale per ogni blocco in base al rapporto tra `Value` e `TotalValue`.
- **Hover e Popup**: Passare il mouse su uno spicchio lo allarga leggermente per evidenziarlo, e fa apparire un elegante riquadro informativo (Popup) che ne descrive il contenuto esatto.
- **Spazio Vuoto**: Se la somma dei valori inseriti non raggiunge il `TotalValue`, il componente disegna automaticamente una barra grigio-chiaro per lo spazio rimanente ("Free Space").

### Come Usarlo
```xml
<sui:SamsungSegmentedBar TitleText="Spazio Disco" 
                         ValueUnit="GB" TotalValue="512" 
                         Segments="{Binding I_Miei_Dati}" />
```
