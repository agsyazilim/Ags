using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ags.Data
{
    public class AgsLazyLoader: ILazyLoader
    {
        private readonly ILazyLoader _efCoreLazyLoader;
        private readonly ICurrentDbContext _currentContext;

        public AgsLazyLoader(ICurrentDbContext currentContext, IDiagnosticsLogger<DbLoggerCategory.Infrastructure> logger)
        {
            _currentContext = currentContext;
            _efCoreLazyLoader = new LazyLoader(currentContext, logger);
        }
        //private bool ContextIsDisposed => ((IContextDisposedState)_currentContext.Context).IsDisposed;
        private bool ContextIsDisposed => false;
        private bool AutoDetectChangesEnabled
        {
            get
            {
                return _currentContext.Context.ChangeTracker.AutoDetectChangesEnabled;
            }
            set
            {
                _currentContext.Context.ChangeTracker.AutoDetectChangesEnabled = value;
            }
        }

        public void Load(object entity, [CallerMemberName] string navigationName = null)
        {
            if (ContextIsDisposed)
                return;

            var originalChangeTrackingState = AutoDetectChangesEnabled;
            try
            {

                AutoDetectChangesEnabled = false;
                _efCoreLazyLoader.Load(entity, navigationName);
            }
            finally
            {
                if (originalChangeTrackingState)
                    AutoDetectChangesEnabled = true;
            }
        }

        public async Task LoadAsync(object entity, CancellationToken cancellationToken = default(CancellationToken), [CallerMemberName] string navigationName = null)
        {
            if (ContextIsDisposed)
                return;

            var originalChangeTrackingState = AutoDetectChangesEnabled;
            try
            {

                AutoDetectChangesEnabled = false;
                await _efCoreLazyLoader.LoadAsync(entity, cancellationToken, navigationName);
            }
            finally
            {
                if (originalChangeTrackingState)
                    AutoDetectChangesEnabled = true;
            }
        }
    }
}