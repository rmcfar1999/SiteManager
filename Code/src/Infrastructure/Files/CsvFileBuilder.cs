using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.TodoLists.Queries.ExportTodos;
using SiteManager.V4.Infrastructure.Files.Maps;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SiteManager.V4.Infrastructure.Files
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Configuration.RegisterClassMap<TodoItemRecordMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}
