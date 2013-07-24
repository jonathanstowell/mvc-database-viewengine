using Castle.MicroKernel.Lifestyle;

namespace MvcThemable.Core.Castle
{
    public class HybridPerWebRequestScopedAccessor : HybridPerWebRequestScopeAccessor
    {
        public HybridPerWebRequestScopedAccessor()
            : base(new LifetimeScopeAccessor())
        {
        }
    }
}
