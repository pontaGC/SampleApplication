namespace SimpleCalculator.Desktop;

/// <summary>
/// Interaction logic for <see cref="MainPage"/>.xaml.
/// </summary>
public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		this.InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

