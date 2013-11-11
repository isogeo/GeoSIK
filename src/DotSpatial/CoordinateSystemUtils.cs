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
using System.Linq;
using System.Text;
using DsProjections=DotSpatial.Projections;

namespace GeoSik.DotSpatial
{

    internal static class CoordinateSystemUtils
    {

        public static DsProjections.ProjectionInfo Convert(ICoordinateSystem system)
        {
            if (system==null)
                return null;

            var cs=system as CoordinateSystem;
            if (cs==null)
                return DsProjections.ProjectionInfo.FromEsriString(system.ToString());

            return cs.Projection;
        }
    }
}
