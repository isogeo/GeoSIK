[![Build status](https://ci.appveyor.com/api/projects/status/fpurmd7uhom6endc/branch/master?svg=true)](https://ci.appveyor.com/project/mcartoixa/geosik/branch/master)
[![Build status](https://ci.appveyor.com/api/projects/status/o7kb3urokq7d8fm2?svg=true)](https://ci.appveyor.com/project/GutsJM/geosik)  
[![Build Status](https://dev.azure.com/isogeo/Isogeo%20API/_apis/build/status/isogeo.GeoSIK?branchName=master)](https://dev.azure.com/isogeo/Isogeo%20API/_build/latest?definitionId=42&branchName=master)


PREREQUISITES
-------------

- Build:
	* Microsoft .NET Framework 4.0
	* [ASP.NET MVC 3](http://www.asp.net/mvc/mvc3)
	* [MSBuild.Community.Tasks 1.5.0](https://github.com/loresoft/msbuildtasks)
	* [OpenCover 4.6](https://github.com/OpenCover/opencover)

- Development:
	* Microsoft Visual Studio 2015
	* [Microsoft SQL Server 2012 Express](http://www.microsoft.com/en-us/download/details.aspx?id=29062)
	* [Sandcastle Help File Builder 2015.6.5.0](https://github.com/EWSoftware/SHFB)



BUILD
-----

- Run the following command from this directory: `build.bat`



DEVELOPMENT
-----------

- The GeoSik.dev.sln solution is meant for development. Other solutions are used by the build scripts.
- Serialization:
	* Serialization objects found in the Library project have been generated with [LinqToXsd](http://linqtoxsd.codeplex.com/) with a command line like the following: `LinqToXsd.exe iso\19139\20070417\gmd\gmd.xsd /config:LinqToXsd.config.xml /enableServiceReference`
	* Original XML schemas can be found online at http://schemas.opengis.net/ or http://eden.ign.fr/
	* Code generation has not been made part of the build as:
		. Generation is somewhat buggy and some generated objects have been manually tweaked.
		. Sometimes the schemas themselves have been tweaked for the generation to work...
	* LinqToXsd configuration looks like the following:
```xml
<Configuration xmlns="http://www.microsoft.com/xml/schema/linq">
  <Namespaces>
    <Namespace Schema="http://www.opengis.net/gml" Clr="GeoSik.Ogc.Gml.V311" />
    <Namespace Schema="http://www.opengis.net/gml/3.2" Clr="GeoSik.Ogc.Gml.V321" />
    <Namespace Schema="http://www.opengis.net/ows" Clr="GeoSik.Ogc.Ows.V100.Types" />
    <Namespace Schema="http://www.opengis.net/ogc" Clr="GeoSik.Ogc.Filter.V110" />
    <Namespace Schema="http://www.opengis.net/cat/csw/2.0.2" Clr="GeoSik.Ogc.WebCatalog.Csw.V202.Types" />
    <Namespace Schema="http://www.isotc211.org/2005/gco" Clr="GeoSik.Iso.Ts19139.Gco" />
    <Namespace Schema="http://www.isotc211.org/2005/gmd" Clr="GeoSik.Iso.Ts19139.Gmd" />
    <Namespace Schema="http://www.isotc211.org/2005/gmx" Clr="GeoSik.Iso.Ts19139.Gmx" />
    <Namespace Schema="http://www.isotc211.org/2005/gsr" Clr="GeoSik.Iso.Ts19139.Gsr" />
    <Namespace Schema="http://www.isotc211.org/2005/gss" Clr="GeoSik.Iso.Ts19139.Gss" />
    <Namespace Schema="http://www.isotc211.org/2005/gts" Clr="GeoSik.Iso.Ts19139.Gts" />
    <Namespace Schema="http://www.isotc211.org/2005/srv" Clr="GeoSik.Iso.Ts19139.Srv" />
    <Namespace Schema="http://www.w3.org/2001/SMIL20/" Clr="GeoSik.W3.Smil20" />
    <Namespace Schema="http://www.w3.org/2001/SMIL20/Language" Clr="GeoSik.W3.Smil20.Language" />
    <Namespace Schema="http://purl.org/dc/elements/1.1/" Clr="GeoSik.DublinCore.Elements.V11" />
    <Namespace Schema="http://purl.org/dc/terms/" Clr="GeoSik.DublinCore.Terms" />
  </Namespaces>
</Configuration>
```