namespace NemSei;
using FFImageLoading.Maui;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
		player = new Player(imgtatu);
		player.Run();
	}


	int count = 0;
	bool EstaMorto = false;
	bool EstaPulando = false;
	bool EstaNoChao = true;
	bool EstaNoAr = false;
	const int TempoEntreFrames = 25;
	int Velocidade1 = 0;
	int Velocidade2 = 0;
	int Velocidade3 = 0;
	int Velocidade = 0;
	const int forcaGravidade = 10;
	int LarguraJanela = 0;
	int AlturaJanela = 0;
	int TempoPulando = 0;
	int TempoNoAr = 5;
	const int ForcaPulo = 20;
	const int maxTempoPulando = 6;
	const int maxTempoNoAr = 10;
	Player player;
	Inimigo inimigo;
	Inimigos inimigos;

	protected override void OnSizeAllocated(Double w, Double h)
	{
		base.OnSizeAllocated(w, h);
		CorrigeTamanhoCenario(w, h);
		CalculaVelocidade(w);
		inimigos = new Inimigos(-w);
		inimigos.Add(new Inimigo(imgInimigo));
		inimigos.Add(new Inimigo(imgInimigo2));
		inimigos.Add(new Inimigo(imgInimigo3));
	}

	void CalculaVelocidade(Double w)
	{
		Velocidade = (int)(w * 0.01);
		Velocidade1 = (int)(w * 0.001);
		Velocidade2 = (int)(w * 0.004);
		Velocidade3 = (int)(w * 0.008);
	}

	void CorrigeTamanhoCenario(Double w, Double h)
	{
		foreach (var a in HSLayer1.Children)
			(a as Image).WidthRequest = w;
		foreach (var l in HSLayer2.Children)
			(l as Image).WidthRequest = w;
		foreach (var k in HSLayer3.Children)
			(k as Image).WidthRequest = w;
		foreach (var i in HSLayerChao.Children)
			(i as Image).WidthRequest = w;

		HSLayer1.WidthRequest = w * 1.5;
		HSLayer2.WidthRequest = w * 1.5;
		HSLayer3.WidthRequest = w * 1.5;
		HSLayerChao.WidthRequest = w * 1.5;
	}

	void GerenciaCenario()
	{
		MoveCenario();
		GerenciaCenario(HSLayer1);
		GerenciaCenario(HSLayer2);
		GerenciaCenario(HSLayer3);
		GerenciaCenario(HSLayerChao);

	}

	void MoveCenario()
	{
		HSLayer1.TranslationX -= Velocidade1;
		HSLayer2.TranslationX -= Velocidade2;
		HSLayer3.TranslationX -= Velocidade3;
		HSLayerChao.TranslationX -= Velocidade;
	}

	async Task Desenha()
	{
		while (!EstaMorto)
		{
			GerenciaCenario();
			if (inimigos != null)
				inimigos.Desenha(Velocidade);
		
		if (!EstaPulando && !EstaNoAr)
		{
			AplicaGravide();
			player.Desenha();
		}
		else
		
			AplicaPulo();
			await Task.Delay(TempoEntreFrames);
		}
	}

	void GerenciaCenario(HorizontalStackLayout HSL)
	{
		var view = (HSL.Children.First() as Image);
		if (view.WidthRequest + HSL.TranslationX < 0)
		{
			HSL.Children.Remove(view);
			HSL.Children.Add(view);
			HSL.TranslationX = view.TranslationX;
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Desenha();
	}

	void AplicaGravide()
	{
		if (player.GetY() < 0)
			player.MoveY(forcaGravidade);
		else if (player.GetY() > 0)
		{
			player.SetY(0);
			EstaNoChao = true;
		}
	}

	void AplicaPulo()
	{
		EstaNoChao = false;
		if (EstaPulando && TempoPulando >= maxTempoPulando)
		{
			EstaPulando = false;
			EstaNoAr = false;
			TempoNoAr = 0;
		}
		else if (EstaNoAr && TempoNoAr >= maxTempoNoAr)
		{
			EstaPulando = false;
			EstaNoAr = false;
			TempoPulando = 0;
			TempoNoAr = 0;
		}
		else if (EstaPulando && TempoPulando < maxTempoPulando)
		{
			player.MoveY(-ForcaPulo);
			TempoPulando++;
		}
		else if (EstaNoAr)
			TempoNoAr++;
	}

	void OnGridTapped(object o, TappedEventArgs a)
	{
		if (EstaNoChao)
			EstaPulando = true;
	}

}




