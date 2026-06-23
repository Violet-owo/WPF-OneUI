# SamsungTreeView & SamsungTreeViewItem

The `SamsungTreeView` component is an essential UI element for management and enterprise applications. It provides a hierarchical list structure, perfect for displaying folder directories, organizational charts, categories, or complex settings.

## Features

- **Fluid Expanding Animation**: The expand/collapse chevron arrow features a smooth 90-degree rotation animation when toggled.
- **One UI Selection Style**: Selected items feature the signature rounded corner bounding box with the solid primary color.
- **Clean Hover State**: Unselected items receive a soft grey rounded background on hover, avoiding the rigid, edge-to-edge highlights found in standard WPF TreeViews.
- **Deep Nesting**: Fully supports infinite levels of hierarchical nesting natively.

## Properties

### SamsungTreeViewItem

| Property | Type | Default Value | Description |
|---|---|---|---|
| `CornerRadius` | `CornerRadius` | `8` | The corner radius of the individual item's selection and hover background. |

*(Note: Inherits all properties from standard WPF `TreeViewItem`, such as `Header` and `IsExpanded`)*.

## Example Usage

### XAML

```xml
<sui:SamsungTreeView>
    <sui:SamsungTreeViewItem Header="Documents" IsExpanded="True">
        <sui:SamsungTreeViewItem Header="Invoices 2026"/>
        <sui:SamsungTreeViewItem Header="Receipts"/>
        
        <sui:SamsungTreeViewItem Header="Archived">
            <sui:SamsungTreeViewItem Header="2024"/>
            <sui:SamsungTreeViewItem Header="2025"/>
        </sui:SamsungTreeViewItem>
    </sui:SamsungTreeViewItem>
    
    <sui:SamsungTreeViewItem Header="Projects">
        <sui:SamsungTreeViewItem Header="Alpha Project"/>
        <sui:SamsungTreeViewItem Header="Beta Project"/>
    </sui:SamsungTreeViewItem>
    
    <sui:SamsungTreeViewItem Header="Settings"/>
</sui:SamsungTreeView>
```

## Best Practices

- Place `SamsungTreeView` inside a `SamsungCard` on the left side of your layout to create a "Sidebar Navigation" pattern commonly used in File Explorers or ERP software.
- Avoid using massive datasets without virtualization if possible, though the standard WPF ItemsPresenter will handle moderate hierarchies very well.
