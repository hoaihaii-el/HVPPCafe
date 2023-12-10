namespace HVPPCafe;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Nunito-Medium.ttf", "NunitoMedium");
				fonts.AddFont("Nunito-MediumItalic.ttf", "NunitoMediumItalic");
				fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
				fonts.AddFont("Nunito-SemiBoldItalic.ttf", "NunitoSemiBoldItalic");
				fonts.AddFont("Nunito-Bold.ttf", "NunitoBoldItalic");
				fonts.AddFont("Nunito-BoldItalic.ttf", "NunitoBoldItalic");
				fonts.AddFont("fa-solid-900.ttf", "AwesomeIcon");
			});

		return builder.Build();
	}
}
