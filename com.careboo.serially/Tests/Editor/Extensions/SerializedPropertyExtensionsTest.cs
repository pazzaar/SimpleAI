﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CareBoo.Serially.Editor.Tests
{
    public class SerializedPropertyExtensionsTest : ScriptableObject
    {
        [Serializable]
        public class TestClass
        {
            public int IntField;
            public int[] ArrayField;
        }

        [Serializable] public class SerializableChildClass : TestClass { }

        public class NonSerializableChildClass : TestClass { }


        [SerializeReference]
        public TestClass SerializeReferenceField;

        public (SerializedProperty, Type) GetNewFixtureProperty(TestClass setFieldValue = null)
        {
            var serializedObject = GetSerializedObject(setFieldValue);
            var property = serializedObject.FindProperty(nameof(SerializeReferenceField));
            var propertyType = typeof(TestClass);
            return (property, propertyType);
        }

        public SerializedObject GetSerializedObject(TestClass setFieldValue = null)
        {
            var fixture = CreateInstance<SerializedPropertyExtensionsTest>();
            fixture.SerializeReferenceField = setFieldValue;
            return new SerializedObject(fixture);
        }

        [Test]
        public void GetManagedReferenceFieldTypeReturnsFieldType()
        {
            var (serializedProperty, expected) = GetNewFixtureProperty();
            var actual = serializedProperty.GetManagedReferenceFieldType();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetManagedReferenceValueTypeReturnsValueType()
        {
            var (serializedProperty, _) = GetNewFixtureProperty(new SerializableChildClass());
            var expected = typeof(SerializableChildClass);
            var actual = serializedProperty.GetManagedReferenceValueType();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetManagedReferenceValueToNewSetsNonNullValue()
        {
            var (serializedProperty, _) = GetNewFixtureProperty();
            serializedProperty.SetManagedReferenceValueToNew(typeof(SerializableChildClass));
            Assert.IsNotNull(serializedProperty.GetValue());
        }

        [Test]
        public void GetSelectableManagedReferenceValueTypesReturnsSerializableDerivedClasses()
        {
            var expected = new HashSet<Type>(new[] { typeof(TestClass), typeof(SerializableChildClass) });
            var (serializedProperty, _) = GetNewFixtureProperty();
            var actual = new HashSet<Type>(serializedProperty.GetSelectableManagedReferenceValueTypes());
            Assert.AreEqual(expected, actual);
        }

        public class GetValueCase<TExpected>
        {
            private readonly string description;

            public TExpected Expected { get; }
            public TestClass SetValue { get; }
            public string PropertyPath { get; }
            public Func<IEnumerable<string>, IEnumerable<string>> PathModifier { get; }

            public GetValueCase(
                string description,
                TExpected expected,
                Func<TExpected, TestClass> setValue,
                string propertyPath,
                Func<IEnumerable<string>, IEnumerable<string>> pathModifier = null)
            {
                this.description = description;
                Expected = expected;
                SetValue = setValue(Expected);
                PropertyPath = propertyPath;
                PathModifier = pathModifier;
            }

            public override string ToString() => description;
        }

        public static object[] GetValueCases = new object[]
        {
            new object[]
            {
                new GetValueCase<int> (
                    "Get Array Element Value",
                    expected: 1,
                    setValue: _ => new TestClass() { ArrayField = new[] { 1, 2 } },
                    propertyPath: "SerializeReferenceField.ArrayField.Array.data[0]"
                )
            },
            new object[]
            {
                new GetValueCase<int[]> (
                    "Get Array Value",
                    expected: new[] { 1, 2 },
                    setValue: expected => new TestClass() { ArrayField = expected },
                    propertyPath: "SerializeReferenceField.ArrayField"
                )
            },
            new object[]
            {
                new GetValueCase<int[]> (
                    "Get Parent Field Value with Path Modifier",
                    expected: new[] { 1, 2 },
                    setValue: expected => new TestClass() { ArrayField = expected },
                    propertyPath: "SerializeReferenceField.ArrayField.Array.data[0]",
                    pathModifier: path => path.SkipLast(1)
                )
            }
        };

        [Test]
        [TestCaseSource(nameof(GetValueCases))]
        public void GetValueReturnsPropertyValue<TExpected>(GetValueCase<TExpected> testCase)
        {
            var serializedObject = GetSerializedObject(testCase.SetValue);
            var serializedProperty = serializedObject.FindProperty(testCase.PropertyPath);
            var actual = serializedProperty.GetValue(testCase.PathModifier);
            Assert.AreEqual(testCase.Expected, actual);
        }
    }
}
