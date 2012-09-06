﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
using System.Reflection;

using NUnit.Framework;

using NGettext;

namespace Tests
{
	[TestFixture]
	public class TranslatorTest
	{
		public string LocalesDir;

		[SetUp]
		public void Init()
		{
			this.LocalesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestResources", "locales");
		}

		[Test]
		public void TestEmpty()
		{
			var t = new Translator();

			Assert.AreEqual(0, t.Translations.Count);
			Assert.AreEqual(CultureInfo.CurrentUICulture, t.CultureInfo);

			t = new Translator(CultureInfo.CreateSpecificCulture("fr"));
			Assert.AreEqual(CultureInfo.CreateSpecificCulture("fr"), t.CultureInfo);
		}

		[Test]
		public void TestStream()
		{
			using (var stream = File.OpenRead(Path.Combine(this.LocalesDir, "ru_RU", "LC_MESSAGES", "Test.mo")))
			{
				var t = new Translator(stream, CultureInfo.CreateSpecificCulture("ru-RU"));
				this._TestLoadedTranslation(t);
			}
		}

		[Test]
		public void TestLocaleDir()
		{
			var t = new Translator("Test", this.LocalesDir, CultureInfo.CreateSpecificCulture("ru-RU"));
			this._TestLoadedTranslation(t);
		}

		private void _TestLoadedTranslation(ITranslator t)
		{
			Assert.AreEqual("тест", t._("test"));
			Assert.AreEqual("тест2", t._("test2"));
			Assert.AreEqual("1 минута", t._n("{0} minute", "{0} minutes", 1, 1));
			Assert.AreEqual("5 минут", t._n("{0} minute", "{0} minutes", 5, 5));

			Assert.AreEqual("тест3контекст1", t._p("context1", "test3"));
			Assert.AreEqual("тест3контекст2", t._p("context2", "test3"));
		}

	}
}