# Conformity

A simple command line application to generate reports or documents from MS-SQL stored procedures. Templates for the reports are defined in HTML as supported by [wkhtmltopdf](https://wkhtmltopdf.org/) and values filled based on [mustache](http://mustache.github.io/).

## How it works

the work to be done is defined in a Jobfile. This is a JSON file with a format as below:

```json
{
  "ConnectionString": "Server=localhost;Database=WideWorldImporters;User Id=sa;Password=P@55w0rd!",
  "PrimaryProcedure": "pr_GetSuppliers",
  "PrimaryKey": "SupplierID",
  "SecondaryProcedures": {
    "SupplierItems": "pr_GetSupplierItems"
  },
  "TemplateFile": "WideWorldImporters_SuppliersReport.html"
}
```

### Jobfile fields

#### ConnectionString

The connection string to the database from which to retrieve the data.

#### PrimaryProcedure

The stored procedure to run to obtain the main data set. This does not support input parameters.
Column names in the result set are matched to the mustache fields in the templates. These matches are case sensitive.

#### PrimaryKey

The column in the result set to use as input to the `SecondaryProcedures`. It is also used as the file name for the resultant pdf.

#### SecondaryProcedures

An optional list of Secondary procedures to be run for each result in the Primary result set. The input to these procedures is the value for the column specified by the `PrimaryKey`. They take the form of:

```json
"MustacheVariableName": "StoredProcedureName"
```

The each result of these SecondaryProcedures are expected to be used in a mustache Section. See [http://mustache.github.io/mustache.5.html](http://mustache.github.io/mustache.5.html)

#### TemplateFile

The path to the HTML/mustache template to use to generate the report. If it is not an absolute path, it is taken as relative to the Jobfile.

Resources in the html template (for example images) are relative to the HTML template file.

## Example report

An example jobfile, template and supporting stored procedures can be found in the `samples` folder. It is intended to be run against the [World Wide Importers](https://github.com/Microsoft/sql-server-samples/releases/tag/wide-world-importers-v1.0) OLTP database.

## Acknowledgements and licenses

This tool makes use of compiled versions of [wkhtmltopdf](https://wkhtmltopdf.org/) which is released under the LGPL license.

Conformity itself is released under an MIT License.