# SamsungNavigationBar

Il `SamsungNavigationBar` (con i suoi `SamsungNavigationBarItem`) emula la barra di navigazione fissa inferiore (Bottom Navigation) presente in moltissime app native. 

![SamsungNavigationBar Example](../Screen/SamsungNavigationBar.png)
> 📸 *Lo screenshot è in pausa caffè! Lo sviluppatore lo caricherà a breve.*

---

## 🇬🇧 English

The `SamsungNavigationBar` emulates the sticky Bottom Navigation bar found in many native mobile apps. It features clean icons and pill-shaped highlighting for the active tab.

### Inheritance
- `SamsungNavigationBar` inherits from `System.Windows.Controls.TabControl`.
- `SamsungNavigationBarItem` inherits from `System.Windows.Controls.TabItem`.

### Custom Properties

| Property (Item) | Type | Default Value | Description |
|-----------|------|-------------------|-------------|
| **Icon** | `string` | `""` | A geometric icon representation (e.g., Segoe MDL2 or Material icon glyphs) rendered above the text. |
| **Text** | `string` | `""` | The label text displayed under the icon. |

### Visual Behavior
- **Selected State**: The active icon and text turn bright (Primary Accent), and a vibrant pill-shaped background highlights the item.
- **Layout**: Horizontally stacks the tabs with evenly spaced distribution (`UniformGrid` internally) allowing the navigation bar to stretch perfectly across the window's bottom.

### How to Use
```xml
<sui:SamsungNavigationBar VerticalAlignment="Bottom">
    <sui:SamsungNavigationBarItem Icon="🏠" Text="Home">
        <TextBlock Text="Home Content Here" />
    </sui:SamsungNavigationBarItem>
    
    <sui:SamsungNavigationBarItem Icon="👤" Text="Profile">
        <TextBlock Text="Profile Content Here" />
    </sui:SamsungNavigationBarItem>
</sui:SamsungNavigationBar>
```

---

## 🇮🇹 Italiano

Il `SamsungNavigationBar` emula la barra di navigazione fissa inferiore (Bottom Navigation) presente in moltissime app native. Mette a disposizione uno switch tra viste con icone pulite e sfondi a pillola per le tab attive.

### Ereditarietà
- `SamsungNavigationBar` eredita la sua logica nativa da `System.Windows.Controls.TabControl`.
- `SamsungNavigationBarItem` eredita da `System.Windows.Controls.TabItem`.

### Proprietà Personalizzate

| Proprietà (In Item) | Tipo | Valore di Default | Descrizione |
|-----------|------|-------------------|-------------|
| **Icon** | `string` | `""` | Il simbolo, l'icona font (es. Segoe o Material Design) o l'emoji mostrata sopra il testo. |
| **Text** | `string` | `""` | L'etichetta testuale mostrata sotto l'icona. |

### Comportamento Visivo
- **Stato Selezionato**: Quando un tab è attivo, icona e testo assumono il colore primario. Inoltre, uno sfondo a forma di pillola semi-trasparente appare dietro l'icona per massimizzare la chiarezza visiva.
- **Layout Flessibile**: Suddivide equamente lo spazio orizzontale a prescindere da quanti elementi ci siano (usa internamente una `UniformGrid`), risultando perfetto se ancorato a fine pagina.

### Come Usarlo
```xml
<sui:SamsungNavigationBar VerticalAlignment="Bottom">
    <sui:SamsungNavigationBarItem Icon="🏠" Text="Home">
        <TextBlock Text="Contenuto pagina Home" />
    </sui:SamsungNavigationBarItem>
    
    <sui:SamsungNavigationBarItem Icon="👤" Text="Profilo">
        <TextBlock Text="Contenuto pagina Profilo" />
    </sui:SamsungNavigationBarItem>
</sui:SamsungNavigationBar>
```
