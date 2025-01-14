﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.SqlServer.Bulk.Internal
{
    public class ModificationCommandSetupProvider : IColumnSetupProvider
    {
        private readonly ImmutableList<IColumnSetup> _columns;

        public ModificationCommandSetupProvider(IEnumerable<IReadOnlyModificationCommand> commands)
        {
            var columns = new ConcurrentDictionary<string, IColumnSetup>();

            var grouped = from c in commands
                          from m in c.ColumnModifications
                          group new { m, c } by m.ColumnName into grp
                          select new
                          {
                              ColumnName = grp.Key,
                              ColumnModifications = grp.Select(p => p.m).ToList(),
                              Commands = grp.Select(p => p.c).ToList()
                          };


            foreach (var column in grouped)
            {
                if (TableName == null)
                {
                    TableName = column.Commands.First().TableName;
                    SchemaName = column.Commands.First().Schema;
                }

                var name = column.ColumnName;
                columns.GetOrAdd(name, p => new DelegateColumnSetup(columns.Count, p, column.ColumnModifications.First().Property.ClrType, x => GetColumnValue(x, p), (x, y) => { }, GetDirection(column.ColumnModifications)));
            }

            _columns = columns.Values.ToImmutableList();
        }

        public string SchemaName { get; }

        public string TableName { get; }

        public IEnumerable<IColumnSetup> Build()
        {
            return _columns;
        }

        public void PropagateValues(object entity, IDictionary<IColumnSetup, object> values)
        {
            var command = ((ModificationCommand)entity);

            var buffer = from m in command.ColumnModifications
                         join c in values on m.ColumnName equals c.Key.ColumnName
                         where m.IsRead
                         select c.Value;


            command.PropagateResults(new ValueBuffer(buffer.ToArray()));
        }

        private static object GetColumnValue(object parma, string name)
        {
            var command = parma as ModificationCommand;
            if (command != null)
            {
                foreach (var item in command.ColumnModifications)
                {
                    if (item.ColumnName == name)
                    {
                        var converter = item.Property.GetValueConverter();

                        Func<object, object> convert = p => p;

                        if (converter != null)
                        {
                            convert = p => converter.ConvertToProvider(p);
                        }

                        if (item.UseOriginalValueParameter)
                        {
                            return convert(item.OriginalValue);
                        }

                        if (item.UseCurrentValueParameter)
                        {
                            return convert(item.Value);
                        }

                        return convert(item.Property.GetDefaultValue());
                    }
                }
            }

            return null;
        }

        private static ValueDirection GetDirection(IEnumerable<IColumnModification> modifications)
        {
            var direction = ValueDirection.None;
            var modification = modifications.First();

            if (modification.Entry.EntityState == EntityState.Deleted && modification.IsKey)
            {
                direction = ValueDirection.Write;
            }
            else
            {
                if (modifications.Any(p => p.IsWrite))
                {
                    direction = direction | ValueDirection.Write;
                }

                if (modifications.Any(p => p.IsRead))
                {
                    direction = direction | ValueDirection.Read;
                }
            }

            return direction;
        }
    }
}