namespace NemSei;
using FFImageLoading.Maui;

public delegate void Callback();

public class Player : Animacao
{
    public Player(CachedImageView a) : base(a)
    {
        for (int i = 1; i <= 4; ++i)
            animacao1.Add($"tatu{i.ToString("D2")}.png");
        for (int i = 1; i <= 4; ++i)
            animacao2.Add($"morrido{i.ToString("D2")}.png");

        SetAnimacaoAtiva(1);
    }
    public void Morto()
    {
        loop = false;
        SetAnimacaoAtiva(2);
    }
    public void Run()
    {
        loop = true;
        SetAnimacaoAtiva(1);
        Play();
    }

    public void MoveY(int s)
	{
		imageView.TranslationY += s;
	}
	public double GetY()
	{
		return imageView.TranslationY;
	}
	public void SetY(double a)
	{
		imageView.TranslationY = a;
	}

}