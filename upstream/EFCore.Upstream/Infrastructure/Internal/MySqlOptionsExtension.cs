// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class MySqlOptionsExtension : RelationalOptionsExtension, IDbContextOptionsExtensionWithDebugInfo
    {
        private long? _serviceProviderHash;
        private bool? _rowNumberPaging;
        private string _logFragment;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public MySqlOptionsExtension()
        {
        }

        // NB: When adding new options, make sure to update the copy ctor below.

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected MySqlOptionsExtension([NotNull] MySqlOptionsExtension copyFrom)
            : base(copyFrom)
        {
            _rowNumberPaging = copyFrom._rowNumberPaging;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override RelationalOptionsExtension Clone()
            => new MySqlOptionsExtension(this);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool? RowNumberPaging => _rowNumberPaging;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual MySqlOptionsExtension WithRowNumberPaging(bool rowNumberPaging)
        {
            var clone = (MySqlOptionsExtension)Clone();

            clone._rowNumberPaging = rowNumberPaging;

            return clone;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override long GetServiceProviderHashCode()
        {
            if (_serviceProviderHash == null)
            {
                _serviceProviderHash = (base.GetServiceProviderHashCode() * 397) ^ (_rowNumberPaging?.GetHashCode() ?? 0L);
            }

            return _serviceProviderHash.Value;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["MySql:" + nameof(MySqlDbContextOptionsBuilder.UseRowNumberForPaging)]
                = (_rowNumberPaging?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override bool ApplyServices(IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.AddEntityFrameworkMySql();

            return true;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append(base.LogFragment);

                    if (_rowNumberPaging == true)
                    {
                        builder.Append("RowNumberPaging ");
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }
    }
}
