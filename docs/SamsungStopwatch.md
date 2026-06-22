# SamsungStopwatch

Il `SamsungStopwatch` è un componente visivo altamente complesso che replica un cronometro circolare in stile "Orologio Samsung". Supporta il conteggio dei tempi parziali (Laps) e possiede anche una modalità Picture-in-Picture (PiP).

![SamsungStopwatch Example](../Screen/SamsungStopwatch.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungStopwatch` is a highly complex visual component that replicates a circular stopwatch in the style of the Samsung Clock app. It supports lap tracking and even features a Picture-in-Picture (PiP) mode.

### Inheritance
Inherits from `System.Windows.Controls.Control`. It internally handles a `DispatcherTimer` to calculate time, rotate needles, and manage laps.

### Custom Properties

| Property | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **ElapsedTimeText** | `string` | `"00:00.00"` | The formatted time displayed in the center. |
| **NeedleAngle** | `double` | `0.0` | The angle of the stopwatch needle. |
| **StartButtonText**| `string` | `"Start"` | Changes dynamically to "Stop" or "Resume". |
| **LapButtonText** | `string` | `"Lap"` | Changes to "Reset" when stopped. |
| **IsRunning** | `bool` | `False` | True if the stopwatch is currently ticking. |
| **HasLaps** | `bool` | `False` | True if at least one lap has been recorded. |
| **IsPipVisible** | `bool` | `False` | Opens a tiny, always-on-top window (`SamsungMiniStopwatch`) to track time while the app is minimized. |

### Visual Behavior
- **Circular Dial**: Draws a clock face with ticks. A needle rotates exactly with the milliseconds elapsed.
- **PiP Mode**: When `IsPipVisible` is set to `True`, a floating mini-stopwatch overlay appears.

### How to Use
```xml
<sui:SamsungStopwatch />
```
*(All time logic is handled internally. You just drop the control onto your page!)*

---

## 🇮🇹 Italiano

Il `SamsungStopwatch` è un componente visivo altamente complesso che replica un cronometro circolare in stile "Orologio Samsung". Supporta il conteggio dei tempi parziali (Laps) e possiede anche una modalità Picture-in-Picture (PiP) fluttuante.

### Ereditarietà
Eredita da `System.Windows.Controls.Control`. Gestisce internamente un `DispatcherTimer` per calcolare con precisione il tempo, ruotare la lancetta animata e salvare i tempi parziali.

### Proprietà Personalizzate

| Proprietà | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **ElapsedTimeText** | `string` | `"00:00.00"` | Il testo formattato al centro del cronometro. |
| **NeedleAngle** | `double` | `0.0` | L'angolo della lancetta rossa del cronometro. |
| **StartButtonText**| `string` | `"Start"` | Il testo del bottone primario (diventa "Stop" o "Riprendi"). |
| **LapButtonText** | `string` | `"Lap"` | Il testo del bottone secondario (diventa "Azzera" da fermo). |
| **IsRunning** | `bool` | `False` | Indica se il cronometro sta scorrendo. |
| **HasLaps** | `bool` | `False` | True se è stato salvato almeno un "Giro" (Lap). |
| **IsPipVisible** | `bool` | `False` | Se True, apre una minuscola finestra fluttuante sempre in primo piano (`SamsungMiniStopwatch`). |

### Comportamento Visivo
- **Quadrante Circolare**: Disegna le tacchette dell'orologio e fa ruotare una lancetta in sincronia con i millisecondi.
- **Modalità PiP**: Attivando l'icona in alto a destra, l'interfaccia principale viene sostituita da un piccolo widget esterno fluttuante, perfetto se si chiude la finestra dell'app.

### Come Usarlo
```xml
<sui:SamsungStopwatch />
```
*(Tutta la logica temporale è racchiusa dentro il componente. Ti basta inserirlo nella pagina XAML!)*
