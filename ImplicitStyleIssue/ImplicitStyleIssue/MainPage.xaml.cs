using System.Reflection;

namespace ImplicitStyleIssue;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        var style = new Style(typeof(CustomGridCell)) { Setters = { new Setter { Property = CustomGridCell.BackgroundProperty, Value = Colors.Red } } };
        this.Resources.Add(style);
    }
}


public class CustomGridCell : ContentView
{
    private Style implicitGridCellStyle = new Style(typeof(CustomGridCell));

    private bool isImplicitStyleChecked = false;

    public static new readonly BindableProperty BackgroundProperty =
BindableProperty.Create(nameof(Background), typeof(Brush), typeof(CustomGridCell), new SolidColorBrush(Color.FromRgba(255, 255, 255, 0.001)), BindingMode.Default, null, propertyChanged: OnBackgroundChanged);
    public CustomGridCell()
    {
          this.Background = Colors.Orange;
    }

    private static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var gridCell = bindable as CustomGridCell;
        gridCell.BackgroundColor = ((SolidColorBrush)newValue).Color;
        gridCell = null;
    }

    internal Style ImplicitGridCellStyle
    {
        get
        {
            if (!this.isImplicitStyleChecked)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                var dynamicResources = (Dictionary<BindableProperty, string>)typeof(Element)?.GetRuntimeProperties()?.FirstOrDefault(x => x.Name == "DynamicResources")?.GetValue(this);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                var key = dynamicResources?.Keys.FirstOrDefault();
                this.implicitGridCellStyle = (Style)this.GetValue(key);
                this.isImplicitStyleChecked = true;
            }

            return implicitGridCellStyle;
        }
    }

    public new Brush Background
    {
        get
        {
            return (Brush)GetValue(BackgroundProperty);
        }

        set
        {
            this.SetValue(BackgroundProperty, value);
        }
    }
}

