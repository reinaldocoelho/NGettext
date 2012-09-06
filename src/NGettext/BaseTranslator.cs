﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using NGettext.Plural;

namespace NGettext
{
	/// <summary>
	/// Base translator.
	/// </summary>
	public abstract class BaseTranslator : ITranslator
	{
		/// <summary>
		/// Context glue (&lt;EOT&gt; symbol)
		/// </summary>
		public const char CONTEXT_GLUE = '\u0004';

		/// <summary>
		/// Current catalog locale.
		/// </summary>
		public CultureInfo CultureInfo { get; protected set; }

		/// <summary>
		/// Current plural form processor.
		/// </summary>
		public PluralFormProcessor PluralForm { get; protected set; }

		/// <summary>
		/// Loaded raw translation strings.
		/// (msgctxt&lt;EOT&gt;)msgid => msgstr[]
		/// </summary>
		public Dictionary<string, string[]> Translations { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseTranslator"/> class that has no translations,
		/// with default plural form formulas, using given culture info.
		/// </summary>
		/// <param name="cultureInfo">Locale of this translator.</param>
		protected BaseTranslator(CultureInfo cultureInfo)
		{
			this.CultureInfo = cultureInfo;
			this.PluralForm = PluralFormProcessor.Default;
			this.Translations = new Dictionary<string, string[]>();
		}

		#region ITranslator implementation

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language.
		/// Similar to <c>gettext</c> function.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetString(string text)
		{
			return this._GetString(text, text);
		}

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language.
		/// Similar to <c>gettext</c> function.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetString(string text, params object[] args)
		{
			return String.Format(this._GetString(text, text), args);
		}

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetPluralString(string text, string pluralText, long n)
		{
			return this._GetPluralString(text, text, pluralText, n);
		}

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/>.
		/// Similar to <c>ngettext</c> function.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetPluralString(string text, string pluralText, long n, params object[] args)
		{
			return String.Format(this._GetPluralString(text, text, pluralText, n), args);
		}

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetParticularString(string context, string text)
		{
			return this._GetString(context + CONTEXT_GLUE + text, text);
		}

		/// <summary>
		/// Returns <paramref name="text"/> translated into the selected language using given <paramref name="context"/>.
		/// Similar to <c>pgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetParticularString(string context, string text, params object[] args)
		{
			return String.Format(this._GetString(context + CONTEXT_GLUE + text, text), args);
		}

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
		/// Similar to <c>npgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetParticularPluralString(string context, string text, string pluralText, long n)
		{
			return this._GetPluralString(context + CONTEXT_GLUE + text, text, pluralText, n);
		}

		/// <summary>
		/// Returns the plural form for <paramref name="n"/> of the translation of <paramref name="text"/> using given <paramref name="context"/>.
		/// Similar to <c>npgettext</c> function.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public virtual string GetParticularPluralString(string context, string text, string pluralText, long n, params object[] args)
		{
			return String.Format(this._GetPluralString(context + CONTEXT_GLUE + text, text, pluralText, n), args);
		}

		#region Aliases

		/// <summary>
		/// Alias for <see cref="ITranslator.GetString(string)"/> method.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		public string _(string text)
		{
			return this.GetString(text);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetString(string,object[])"/> method.
		/// </summary>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public string _(string text, params object[] args)
		{
			return this.GetString(text, args);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetPluralString(string,string,long)"/> method.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		public string _n(string text, string pluralText, long n)
		{
			return this.GetPluralString(text, pluralText, n);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetPluralString(string,string,long,object[])"/> method.
		/// </summary>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public string _n(string text, string pluralText, long n, params object[] args)
		{
			return this.GetPluralString(text, pluralText, n, args);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetParticularString(string,string)"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <returns>Translated text.</returns>
		public string _p(string context, string text)
		{
			return this.GetParticularString(context, text);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetParticularString(string,string,object[])"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Text to translate.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public string _p(string context, string text, params object[] args)
		{
			return this.GetParticularString(context, text, args);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetParticularPluralString(string,string,string,long)"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <returns>Translated text.</returns>
		public string _pn(string context, string text, string pluralText, long n)
		{
			return this.GetParticularPluralString(context, text, pluralText, n);
		}

		/// <summary>
		/// Alias for <see cref="ITranslator.GetParticularPluralString(string,string,string,long,object[])"/> method.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="text">Singular form of message to translate.</param>
		/// <param name="pluralText">Plural form of message to translate.</param>
		/// <param name="n">Value that determines the plural form.</param>
		/// <param name="args">Optional arguments for <see cref="System.String.Format(string, object[])"/> method.</param>
		/// <returns>Translated text.</returns>
		public string _pn(string context, string text, string pluralText, long n, params object[] args)
		{
			return this.GetParticularPluralString(context, text, pluralText, n, args);
		}

		#endregion

		#endregion

		/// <summary>
		/// Returns translated string for given <paramref name="messageId"/> or <paramref name="defaultMessage"/> on fail.
		/// </summary>
		/// <param name="messageId">Message ID</param>
		/// <param name="defaultMessage">Default message</param>
		/// <returns>Translated string</returns>
		protected virtual string _GetString(string messageId, string defaultMessage)
		{
			var translations = this._GetTranslations(messageId);

			if (translations == null || translations.Length == 0)
			{
				Trace.WriteLine(String.Format("Translation not found for message id \"{0}\".", messageId), "NGettext");
				return defaultMessage;
			}

			return translations[0];
		}

		/// <summary>
		/// Returns translated plural string for given <paramref name="messageId"/> or 
		/// <paramref name="defaultMessage"/> or <paramref name="defaultPluralMessage"/> on fail.
		/// </summary>
		/// <param name="messageId">Message ID</param>
		/// <param name="defaultMessage">Default message singular form</param>
		/// <param name="defaultPluralMessage">Default message plural form</param>
		/// <param name="n">Value that determines the plural form</param>
		/// <returns>Translated string</returns>
		protected virtual string _GetPluralString(string messageId, string defaultMessage, string defaultPluralMessage, long n)
		{
			var translations = this._GetTranslations(messageId);
			var pluralIndex = this.PluralForm.GetPluralFormIndex(this.CultureInfo, n);

			if (translations == null || translations.Length <= pluralIndex)
			{
				Trace.WriteLine(String.Format("Translation not found (plural) for message id \"{0}\".", messageId), "NGettext");
				return (n == 1) ? defaultMessage : defaultPluralMessage;
			}

			return translations[pluralIndex];
		}

		/// <summary>
		/// Returns all translations for given <paramref name="messageId"/>.
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns>Returns all translations for given <paramref name="messageId"/> or null if not found.</returns>
		protected virtual string[] _GetTranslations(string messageId)
		{
			if (String.IsNullOrEmpty(messageId)) return null;
			if (!this.Translations.ContainsKey(messageId)) return null;

			return this.Translations[messageId];
		}
	}
}