using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static class GUIUtils
{
    public delegate void GenTexture(string name);

    static public Rect DrawOutlineLabel(string text, GUIStyle style, Color outlineColor, Color backgroundColor, Color textColor)
    {
        Rect titleRect = EditorGUILayout.BeginVertical("box");
        Rect baseRect = new Rect(titleRect.x, titleRect.y, EditorGUIUtility.currentViewWidth - 20 - titleRect.x, style.fontSize + 14);
        EditorGUI.DrawRect(baseRect, outlineColor);
        EditorGUI.DrawRect(new Rect(titleRect.x + 2, titleRect.y + 2, EditorGUIUtility.currentViewWidth - 20 - titleRect.x - 4, style.fontSize + 10), backgroundColor);
        var prevColor = style.normal.textColor;
        style.normal.textColor = textColor;
        EditorGUI.LabelField(new Rect(titleRect.x + 10, titleRect.y + 6, EditorGUIUtility.currentViewWidth - 20 - titleRect.x - 4, style.fontSize), text, style);
        style.normal.textColor = prevColor;
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(style.fontSize + 14);

        return baseRect;
    }

    static Dictionary<string, GUIStyle> styles;

    static public GUIStyle GetActionTitleStyle()
    {
        if (styles == null) styles = new Dictionary<string, GUIStyle>();
        
        GUIStyle titleStyle;
        styles.TryGetValue("ActionTitle", out titleStyle);
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 24;
            titleStyle.fixedHeight = 24;
            titleStyle.normal.textColor = ColorFromHex("#0e1a51");
            titleStyle.clipping = TextClipping.Overflow;
            titleStyle.wordWrap = true;
            styles.Add("ActionTitle", titleStyle);
        }
        return titleStyle;
    }

    static public GUIStyle GetButtonStyle(string name, GenTexture gen_texture)
    {
        if (styles == null) styles = new Dictionary<string, GUIStyle>();

        GUIStyle style;
        styles.TryGetValue(name, out style);
        if (style == null)
        {
            style = CreateButtonStyle(name, gen_texture);
            styles.Add(name, style);
        }
        return style;
    }

    static public GUIStyle CreateButtonStyle(string name, GenTexture gen_textures)
    {
        var activeTexture = GetTexture(name + ":normal");
        if (activeTexture == null)
        {
            gen_textures(name);
        }

        var style = new GUIStyle("Button");
        style.normal.background = GetTexture(name + ":normal");
        style.hover.background = GetTexture(name + ":hover");

        return style;
    }

    static public Color ColorFromHex(string htmlColor)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(htmlColor, out color)) return color;

        return Color.magenta;
    }

    static public Texture2D GetColorTexture(string name, Color color)
    {
        var ret = GetTexture(name);
        if (ret != null) return ret;

        var bitmap = new GUIBitmap(4, 4);
        bitmap.Fill(color);

        return BitmapToTexture(name, bitmap);
    }

    static Dictionary<string, Texture2D> textures;
    static public void AddTexture(string name, Texture2D texture)
    {
        if (textures == null) textures = new Dictionary<string, Texture2D>();

        textures[name] = texture;
    }
    static public Texture2D GetTexture(string name)
    {
        if (textures == null) return null;

        Texture2D texture;
        if (textures.TryGetValue(name, out texture))
        {
            return texture;
        }

        return null;
    }

    static public Texture2D BitmapToTexture(string name, GUIBitmap bitmap)
    {
        Texture2D result = new Texture2D(bitmap.width, bitmap.height);
        result.SetPixels(bitmap.bitmap);
        result.filterMode = FilterMode.Point;
        result.Apply();

        if (name != "")
        {
            AddTexture(name, result);
        }

        return result;
    }
}