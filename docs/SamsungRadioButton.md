# SamsungRadioButton

Il `SamsungRadioButton` è l'elemento essenziale per scelte mutuamente esclusive. Presenta un'estetica circolare minimale, in linea con i pattern di design One UI.

![SamsungRadioButton Example](../Screen/SamsungRadioButton.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungRadioButton` is the essential element for mutually exclusive choices. It features a minimal circular aesthetic, in line with One UI design patterns.

### Inheritance
This control inherits directly from the native WPF `System.Windows.Controls.RadioButton` class. 
It supports the standard `GroupName`, `IsChecked`, and `Command` bindings.

### Custom Properties
There are no additional `DependencyProperty` configurations for this control. The modern circular design, along with its inner scaling animation, is fully implemented via the XAML control template.

### Visual Behavior
- **Unchecked**: A subtle circular outline.
- **Checked**: The outline smoothly transitions into a filled circle, and a smaller solid dot scales up from the center using a fluid `DoubleAnimation`.
- **Hover (`IsMouseOver`)**: A translucent circle appears behind the radio button to provide immediate interaction feedback.

### How to Use
```xml
<StackPanel>
    <sui:SamsungRadioButton Content="Option A" GroupName="Group1" IsChecked="True" />
    <sui:SamsungRadioButton Content="Option B" GroupName="Group1" />
</StackPanel>
```

---

## 🇮🇹 Italiano

Il `SamsungRadioButton` è l'elemento essenziale per scelte mutuamente esclusive. Presenta un'estetica circolare minimale, in linea con i pattern di design One UI.

### Ereditarietà
Questo controllo eredita direttamente dalla classe nativa WPF `System.Windows.Controls.RadioButton`.
Supporta nativamente la raggruppazione tramite `GroupName`, i binding di `IsChecked` e i `Command`.

### Proprietà Personalizzate
Non sono state aggiunte nuove `DependencyProperty` a livello di codice. Il design circolare moderno, insieme all'animazione di ridimensionamento del puntino interno, è completamente implementato tramite il template XAML.

### Comportamento Visivo
- **Unchecked**: Un contorno circolare sottile e minimale.
- **Checked**: Il contorno si riempie del colore primario, mentre un puntino solido centrale compare dal nulla allargandosi con una fluida animazione scalare (`DoubleAnimation`).
- **Hover (`IsMouseOver`)**: Un cerchio semitrasparente appare dietro il radio button per fornire un feedback immediato all'interazione del mouse.

### Come Usarlo
```xml
<StackPanel>
    <sui:SamsungRadioButton Content="Opzione A" GroupName="Gruppo1" IsChecked="True" />
    <sui:SamsungRadioButton Content="Opzione B" GroupName="Gruppo1" />
</StackPanel>
```
