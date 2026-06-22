# SamsungChart (Bar & Donut)

Il sistema include due tipologie di grafici altamente animati: `SamsungBarChart` (grafici a barre verticali) e `SamsungDonutChart` (grafici a ciambella circolare).

![SamsungChart Example](../Screen/SamsungChart.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The library includes two highly animated chart types: `SamsungBarChart` (vertical bar charts) and `SamsungDonutChart` (circular pie/donut charts). Both use dynamic tooltips/popups to show data details.

### Inheritance
- Both inherit from `System.Windows.Controls.Control` and use Canvas manipulation or Paths to draw their geometry dynamically.

### Custom Properties (`SamsungDonutChart` as reference)

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **Segments** | `IEnumerable<ChartSegment>`| `null` | The list of data points to render. |
| **CenterText** | `string` | `""` | The large number in the middle of the donut. |
| **CenterSubtext**| `string` | `""` | The small label under the center number. |
| **ValueUnit** | `string` | `""` | The suffix appended to numbers (e.g., "%"). |
| **IsPopupOpen** | `bool` | `False` | Triggers the `SamsungChartPopup` above the chart. |

### Visual Behavior
- **Bar Chart**: Bars grow from the bottom up using `DoubleAnimation`. Hovering over a bar reveals a floating, shadowed `SamsungChartPopup` containing the exact value.
- **Donut Chart**: Draws SVG arcs based on percentages. Hovering over a slice expands it slightly and triggers the popup.

### How to Use
```xml
<!-- The ItemsSource/Segments expects a list of 'ChartSegment' objects -->
<sui:SamsungDonutChart CenterText="85" CenterSubtext="Score"
                       Segments="{Binding MyChartData}" />
```

---

## 🇮🇹 Italiano

Il sistema include due tipologie di grafici personalizzati e altamente animati: `SamsungBarChart` (grafici a barre verticali) e `SamsungDonutChart` (grafici a ciambella circolare). Entrambi si affidano ai pop-up per mostrare i valori esatti.

### Ereditarietà
- Entrambi ereditano da `System.Windows.Controls.Control`. Disegnano i loro elementi dinamicamente: rettangoli via codice per le barre, e percorsi matematici ad arco (SVG Path) per la ciambella.

### Proprietà Personalizzate (`SamsungDonutChart` come riferimento)

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Segments** | `IEnumerable<ChartSegment>`| `null` | La lista dei dati da renderizzare. |
| **CenterText** | `string` | `""` | Il numero grande al centro della ciambella (es. il totale). |
| **CenterSubtext**| `string` | `""` | La descrizione sotto al numero centrale. |
| **ValueUnit** | `string` | `""` | L'unità di misura da accodare (es. "%", "GB"). |
| **IsPopupOpen** | `bool` | `False` | Controlla l'apertura del fumetto informativo (`SamsungChartPopup`). |

### Comportamento Visivo
- **Bar Chart**: Le barre crescono verticalmente dal basso usando una `DoubleAnimation`. Passando il mouse su una barra, appare in cima un `SamsungChartPopup` fluttuante con il valore esatto e un'ombreggiatura netta.
- **Donut Chart**: Disegna archi colorati in base alle percentuali. Sfiorare uno spicchio col mouse lo ingrandisce (espande il raggio) e mostra il popup fluttuante che lo segue.

### Come Usarlo
```xml
<!-- Segments richiede una lista di oggetti di tipo 'ChartSegment' -->
<sui:SamsungDonutChart CenterText="85" CenterSubtext="Punteggio"
                       Segments="{Binding DatiGrafico}" />
```
