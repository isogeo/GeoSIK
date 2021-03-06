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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xml.Schema.Linq;
using LinqExpressionType=System.Linq.Expressions.ExpressionType;

namespace GeoSik.Ogc.Filter.V110
{

#pragma warning disable 1591, 3009
    partial class logicOps:
        IExpressionBuilder
    {

        internal class BinaryLogicalExpressionCreator:
            ExpressionCreator<IBinaryLogicalOperator>
        {

            public BinaryLogicalExpressionCreator(IBinaryLogicalOperator op):
                base(op)
            {
            }

            protected override Expression CreateStandardExpression(IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                Expression ret=null;
                foreach (Expression ex in subexpr)
                {
                    if (ret!=null)
                        ret=Expression.MakeBinary(
                            FilterElement.OperatorExpressionType,
                            ret,
                            ex
                        );
                    else
                        ret=ex;
                }
                return ret;
            }

            protected override string GetCustomImplementationName(List<Type> paramTypes, List<object> paramValues, ExpressionBuilderParameters parameters)
            {
                return FilterElement.OperatorExpressionType.ToString();
            }

            protected override Expression CreateCustomExpression(MethodInfo method, object instance, IEnumerable<Expression> subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                Expression ret=null;
                foreach (Expression exp in subexpr)
                {
                    if (ret!=null)
                    {
                        if (instance!=null)
                            ret=Expression.Call(
                                Expression.Constant(instance),
                                method,
                                ret,
                                exp
                            );
                        else
                            ret=Expression.Call(
                                method,
                                ret,
                                exp
                            );

                        Type rt=Nullable.GetUnderlyingType(method.ReturnType) ?? method.ReturnType;
                        if (method.ReturnType!=typeof(bool))
                            ret=Expression.Equal(
                                ret,
                                Expression.Constant(Convert.ChangeType(true, rt, CultureInfo.InvariantCulture), method.ReturnType)
                            );
                    } else
                        ret=exp;
                }
                return ret;
            }

            protected override IEnumerator<IExpressionBuilder> GetEnumerator()
            {
                return GetLogicalElements(FilterElement).GetEnumerator();
            }

            private static IEnumerable<IExpressionBuilder> GetLogicalElements(IBinaryLogicalOperator op)
            {
                int icmp, ispa, ilog, ifun;
                icmp=ispa=ilog=ifun=0;

                // We want them in the order they were declared...
                foreach (XElement d in ((XTypedElement)op).Untyped.Elements())
                {
                    if ((op.comparisonOps!=null) && (op.comparisonOps.Count>icmp) && (d.Name==op.comparisonOps[icmp].Untyped.Name))
                        yield return op.comparisonOps[icmp++];
                    else if ((op.spatialOps!=null) && (op.spatialOps.Count>ispa) && (d.Name==op.spatialOps[ispa].Untyped.Name))
                        yield return op.spatialOps[ispa++];
                    else if ((op.logicOps!=null) && (op.logicOps.Count>ilog) && (d.Name==op.logicOps[ilog].Untyped.Name))
                        yield return op.logicOps[ilog++];
                    else if ((op.Function!=null) && (op.Function.Count>ifun) && (d.Name==op.Function[ifun].Untyped.Name))
                        yield return op.Function[ifun++];
                    else
                        throw new NotSupportedException();
                }
            }
        }

        internal sealed class UnaryLogicalExpressionCreator:
            IExpressionCreator
        {

            public UnaryLogicalExpressionCreator(IUnaryLogicalOperator op)
            {
                _FilterElement=op;
            }

            public Expression CreateExpression(ExpressionBuilderParameters parameters)
            {
                using (IEnumerator<IExpressionBuilder> children=GetEnumerator())
                    if (children.MoveNext() && (children.Current!=null))
                        return CreateStandardExpression(
                            children.Current.CreateExpression(parameters, children.Current.GetExpressionStaticType(parameters), null),
                            parameters,
                            _FilterElement.GetExpressionStaticType(parameters)
                        );

                return null;
            }

            private Expression CreateStandardExpression(Expression subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                return Expression.MakeUnary(
                    _FilterElement.OperatorExpressionType,
                    subexpr,
                    null
                );
            }

            private string GetCustomImplementationName(List<Type> paramTypes, List<object> paramValues, ExpressionBuilderParameters parameters)
            {
                return _FilterElement.OperatorExpressionType.ToString();
            }

            private Expression CreateCustomExpression(MethodInfo method, object instance, Expression subexpr, ExpressionBuilderParameters parameters, Type subType)
            {
                throw new NotSupportedException();
                //Expression ret=null;

                //if (instance!=null)
                //    ret=Expression.Call(
                //        Expression.Constant(instance),
                //        method,
                //        subexpr
                //    );
                //else
                //    ret=Expression.Call(
                //        method,
                //        subexpr
                //    );

                //Type rt=Nullable.GetUnderlyingType(method.ReturnType)??method.ReturnType;
                //if (method.ReturnType!=typeof(bool))
                //    ret=Expression.Equal(
                //        ret,
                //        Expression.Constant(Convert.ChangeType(true, rt, CultureInfo.InvariantCulture), method.ReturnType)
                //    );

                //return ret;
            }

            private IEnumerator<IExpressionBuilder> GetEnumerator()
            {
                var ret=new List<IExpressionBuilder>(1);

                if (_FilterElement.logicOps!=null)
                    ret.Add(_FilterElement.logicOps);
                else if (_FilterElement.comparisonOps!=null)
                    ret.Add(_FilterElement.comparisonOps);
                else if (_FilterElement.spatialOps!=null)
                    ret.Add(_FilterElement.spatialOps);
                else if (_FilterElement.Function!=null)
                    ret.Add(_FilterElement.Function);

                return ret.GetEnumerator();
            }

            IEnumerator<IExpressionBuilder> IEnumerable<IExpressionBuilder>.GetEnumerator()
            {
                return GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private IUnaryLogicalOperator _FilterElement;
        }

        internal protected virtual Expression CreateExpression(ExpressionBuilderParameters parameters, Type expectedStaticType, Func<Expression, ParameterExpression, Expression> operatorCreator)
        {
            return GetExpressionCreator().CreateExpression(parameters);
        }

        internal protected virtual Type GetExpressionStaticType(ExpressionBuilderParameters parameters)
        {
            return typeof(bool);
        }

        internal protected virtual IExpressionCreator GetExpressionCreator()
        {
            var blop=this as IBinaryLogicalOperator;
            if (blop!=null)
                return new BinaryLogicalExpressionCreator(blop);

            var ulop=this as IUnaryLogicalOperator;
            if (ulop!=null)
                return new UnaryLogicalExpressionCreator(ulop);

            throw new NotSupportedException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    SR.UnsupportedFilterElement,
                    GetType().Name
                )
            );
        }

        Expression IExpressionBuilder.CreateExpression(ExpressionBuilderParameters parameters, Type expectedStaticType, Func<Expression, ParameterExpression, Expression> operatorCreator)
        {
            return CreateExpression(parameters, expectedStaticType, operatorCreator);
        }

        Type IExpressionBuilder.GetExpressionStaticType(ExpressionBuilderParameters parameters)
        {
            return GetExpressionStaticType(parameters);
        }
    }
#pragma warning restore 1591, 3009
}
