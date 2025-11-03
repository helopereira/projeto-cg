/// <summary>
/// Interface usada para marcar scripts que representam uma 'fase' ou 'objetivo'
/// que o GameProgressManager deve rastrear. Não requer métodos.
/// </summary>
public interface ICompletable
{
    // Esta interface não precisa de métodos, mas garante que a classe
    // que a implementa pode ser encontrada pelo GameProgressManager.
}
