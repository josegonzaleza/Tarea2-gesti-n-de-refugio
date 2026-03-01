namespace Tarea2.Services
{
    public static class DataStore
    {
        public static void EnsureSeed()
        {
            new JsonUserStore().EnsureSeed();
            new JsonMedsStore().EnsureSeed();
        }
    }
}