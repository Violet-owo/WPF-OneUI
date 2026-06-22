# SamsungDateTimePicker

Il `SamsungDateTimePicker` è un componente estremamente ricco per la selezione di date e orari. Sostituisce il vetusto `DatePicker` di WPF, introducendo un menu a tendina enorme, animato, e navigabile sia con scorrimento che con bottoni.

![SamsungDateTimePicker Example](../Screen/SamsungDateTimePicker.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungDateTimePicker` is an extremely rich component for selecting dates and times. It completely replaces the outdated WPF `DatePicker`, introducing an oversized, animated popup dropdown that can be navigated by scrolling or clicking.

### Inheritance
This control inherits from `System.Windows.Controls.Control` and builds a completely custom UI for both the input field and the complex popup.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **SelectedDate** | `DateTime?` | `null` | The currently selected date/time. |
| **PickerMode** | `Enum` | `DateTime` | Determines what to pick: `DateOnly`, `TimeOnly`, or `DateTime`. |
| **ShowSeconds** | `bool` | `False` | When in time-picking mode, toggles the visibility of the seconds column. |
| **PlaceholderText**| `string` | `"Select date..."`| Text shown when no date is selected. |
| **CurrentDisplayMonth** | `DateTime` | `Today` | The month currently displayed in the calendar view. |
| **ViewMode** | `CalendarViewMode`| `Month` | Defines if the calendar is showing Days, Months, or Years. |

### Visual Behavior
- **Popup Animation**: When opened, the massive popup card slides down softly with a fade-in effect.
- **Calendar Grid**: Fully redesigned with perfectly circular selection halos and modern typography.
- **Time Wheels**: Uses scrolling list boxes (spinners) that mimic mobile time pickers.

### How to Use
```xml
<sui:SamsungDateTimePicker PickerMode="DateTime" 
                           SelectedDate="{Binding MyDate}" />
```

---

## 🇮🇹 Italiano

Il `SamsungDateTimePicker` è un componente estremamente ricco per la selezione di date e orari. Sostituisce il vetusto `DatePicker` di WPF, introducendo un menu a tendina (Popup) enorme, animato e navigabile, ispirato all'app Calendario di Samsung.

### Ereditarietà
Questo controllo eredita da `System.Windows.Controls.Control` e costruisce un'interfaccia completamente su misura sia per il campo di testo (toggle) sia per il complesso popup del calendario/orologio.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **SelectedDate** | `DateTime?` | `null` | La data e l'orario attualmente selezionati. |
| **PickerMode** | `Enum` | `DateTime` | Determina cosa selezionare: `DateOnly`, `TimeOnly`, o `DateTime`. |
| **ShowSeconds** | `bool` | `False` | Se la selezione dell'orario è attiva, mostra o nasconde la colonna dei secondi. |
| **PlaceholderText**| `string` | `"Select date..."`| Testo segnaposto quando nessuna data è selezionata. |
| **CurrentDisplayMonth** | `DateTime` | `Today` | Il mese attualmente mostrato nella griglia del calendario. |
| **ViewMode** | `CalendarViewMode`| `Month` | Determina se il calendario sta mostrando i Giorni (Month), i Mesi (Year) o gli Anni (Decade). |

### Comportamento Visivo
- **Animazione Popup**: All'apertura, il grande pannello scivola fluidamente verso il basso in dissolvenza.
- **Griglia Calendario**: Completamente ridisegnata, usa aloni di selezione perfettamente circolari (stile pillola) e font moderni.
- **Ruote Orarie**: Utilizza delle liste a scorrimento infinito (spinners) per emulare il feeling dei selettori orari degli smartphone.

### Come Usarlo
```xml
<sui:SamsungDateTimePicker PickerMode="DateTime" 
                           SelectedDate="{Binding MyDate}" />
```
