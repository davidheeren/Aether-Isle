#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED

void GetTextureSize_float(Texture2D tex, out float width, out float height, out float texelWidth, out float texelHeight)
{
    uint w, h;
    tex.GetDimensions(w, h);
    
    width = w;
    height = h;
    
    texelWidth = 1.0 / w;
    texelHeight = 1.0 / h;
}

#endif // ADDITIONAL_LIGHT_INCLUDED