namespace Demo_T1.Services
{
    public class SomeService : ITransientService, IScopedService, ISingletonService
    {
        Guid id;
        public SomeService()
        {
            id = Guid.NewGuid();
        }

        public Guid GetID()
        {
            return id;
        }
    }
}
