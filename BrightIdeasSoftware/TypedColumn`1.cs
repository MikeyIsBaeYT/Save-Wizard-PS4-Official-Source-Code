// Decompiled with JetBrains decompiler
// Type: BrightIdeasSoftware.TypedColumn`1
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BrightIdeasSoftware
{
  public class TypedColumn<T> where T : class
  {
    private OLVColumn column;
    private TypedColumn<T>.TypedAspectGetterDelegate aspectGetter;
    private TypedColumn<T>.TypedAspectPutterDelegate aspectPutter;
    private TypedColumn<T>.TypedImageGetterDelegate imageGetter;
    private TypedColumn<T>.TypedGroupKeyGetterDelegate groupKeyGetter;

    public TypedColumn(OLVColumn column) => this.column = column;

    public TypedColumn<T>.TypedAspectGetterDelegate AspectGetter
    {
      get => this.aspectGetter;
      set
      {
        this.aspectGetter = value;
        if (value == null)
          this.column.AspectGetter = (AspectGetterDelegate) null;
        else
          this.column.AspectGetter = (AspectGetterDelegate) (x => x == null ? (object) null : this.aspectGetter((T) x));
      }
    }

    public TypedColumn<T>.TypedAspectPutterDelegate AspectPutter
    {
      get => this.aspectPutter;
      set
      {
        this.aspectPutter = value;
        if (value == null)
          this.column.AspectPutter = (AspectPutterDelegate) null;
        else
          this.column.AspectPutter = (AspectPutterDelegate) ((x, newValue) => this.aspectPutter((T) x, newValue));
      }
    }

    public TypedColumn<T>.TypedImageGetterDelegate ImageGetter
    {
      get => this.imageGetter;
      set
      {
        this.imageGetter = value;
        if (value == null)
          this.column.ImageGetter = (ImageGetterDelegate) null;
        else
          this.column.ImageGetter = (ImageGetterDelegate) (x => this.imageGetter((T) x));
      }
    }

    public TypedColumn<T>.TypedGroupKeyGetterDelegate GroupKeyGetter
    {
      get => this.groupKeyGetter;
      set
      {
        this.groupKeyGetter = value;
        if (value == null)
          this.column.GroupKeyGetter = (GroupKeyGetterDelegate) null;
        else
          this.column.GroupKeyGetter = (GroupKeyGetterDelegate) (x => this.groupKeyGetter((T) x));
      }
    }

    public void GenerateAspectGetter()
    {
      if (string.IsNullOrEmpty(this.column.AspectName))
        return;
      this.AspectGetter = this.GenerateAspectGetter(typeof (T), this.column.AspectName);
    }

    private TypedColumn<T>.TypedAspectGetterDelegate GenerateAspectGetter(
      Type type,
      string path)
    {
      DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof (object), new Type[1]
      {
        type
      }, type, true);
      this.GenerateIL(type, path, dynamicMethod.GetILGenerator());
      return (TypedColumn<T>.TypedAspectGetterDelegate) dynamicMethod.CreateDelegate(typeof (TypedColumn<T>.TypedAspectGetterDelegate));
    }

    private void GenerateIL(Type type, string path, ILGenerator il)
    {
      il.Emit(OpCodes.Ldarg_0);
      string[] strArray = path.Split('.');
      for (int index = 0; index < strArray.Length; ++index)
      {
        type = this.GeneratePart(il, type, strArray[index], index == strArray.Length - 1);
        if (type == (Type) null)
          break;
      }
      if (type != (Type) null && type.IsValueType && !typeof (T).IsValueType)
        il.Emit(OpCodes.Box, type);
      il.Emit(OpCodes.Ret);
    }

    private Type GeneratePart(ILGenerator il, Type type, string pathPart, bool isLastPart)
    {
      MemberInfo memberInfo = new List<MemberInfo>((IEnumerable<MemberInfo>) type.GetMember(pathPart)).Find((Predicate<MemberInfo>) (x =>
      {
        if (x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property)
          return true;
        return x.MemberType == MemberTypes.Method && ((MethodBase) x).GetParameters().Length == 0;
      }));
      if (memberInfo == (MemberInfo) null)
      {
        il.Emit(OpCodes.Pop);
        if (Munger.IgnoreMissingAspects)
          il.Emit(OpCodes.Ldnull);
        else
          il.Emit(OpCodes.Ldstr, string.Format("'{0}' is not a parameter-less method, property or field of type '{1}'", (object) pathPart, (object) type.FullName));
        return (Type) null;
      }
      Type localType = (Type) null;
      switch (memberInfo.MemberType)
      {
        case MemberTypes.Field:
          FieldInfo field = (FieldInfo) memberInfo;
          il.Emit(OpCodes.Ldfld, field);
          localType = field.FieldType;
          break;
        case MemberTypes.Method:
          MethodInfo meth = (MethodInfo) memberInfo;
          if (meth.IsVirtual)
            il.Emit(OpCodes.Callvirt, meth);
          else
            il.Emit(OpCodes.Call, meth);
          localType = meth.ReturnType;
          break;
        case MemberTypes.Property:
          PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
          il.Emit(OpCodes.Call, propertyInfo.GetGetMethod());
          localType = propertyInfo.PropertyType;
          break;
      }
      if (localType.IsValueType && !isLastPart)
      {
        LocalBuilder local = il.DeclareLocal(localType);
        il.Emit(OpCodes.Stloc, local);
        il.Emit(OpCodes.Ldloca, local);
      }
      return localType;
    }

    public delegate object TypedAspectGetterDelegate(T rowObject) where T : class;

    public delegate void TypedAspectPutterDelegate(T rowObject, object newValue) where T : class;

    public delegate object TypedGroupKeyGetterDelegate(T rowObject) where T : class;

    public delegate object TypedImageGetterDelegate(T rowObject) where T : class;
  }
}
