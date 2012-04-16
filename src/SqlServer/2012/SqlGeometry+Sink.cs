﻿////////////////////////////////////////////////////////////////////////////////
//
// This file is part of GeoSIK.
// Copyright (C) 2012 Isogeo
//
// GeoSIK is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
//
// GeoSIK is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with GeoSIK. If not, see <http://www.gnu.org/licenses/>.
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProjNet.CoordinateSystems;
using SqlTypes=Microsoft.SqlServer.Types;

namespace GeoSik.SqlServer
{

    partial class SqlGeometry
    {

        internal class Sink:
            SqlTypes.IGeometrySink110
        {

            public Sink(IGeometrySink sink)
            {
                Debug.Assert(sink!=null);
                if (sink==null)
                    throw new ArgumentNullException("sink");

                _Sink=sink;
            }

            void SqlTypes.IGeometrySink110.AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
            {
                throw new NotSupportedException();
            }

            public void AddLine(double x, double y, double? z, double? m)
            {
                _Sink.AddLine(x, y, z);
            }

            public void BeginFigure(double x, double y, double? z, double? m)
            {
                _Sink.BeginFigure(x, y, z);
            }

            public void BeginGeometry(SqlTypes.OpenGisGeometryType type)
            {
                _Sink.BeginGeometry(GeometryTypeUtils.Convert(type));
            }

            public void EndFigure()
            {
                _Sink.EndFigure();
            }

            public void EndGeometry()
            {
                _Sink.EndGeometry();
            }

            public void SetSrid(int srid)
            {
                _Sink.SetCoordinateSystem(
                    CoordinateSystemProvider.Instance.GetById(new Srid(srid))
                );
            }

            private IGeometrySink _Sink;
        }
    }
}
