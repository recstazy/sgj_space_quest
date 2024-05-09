#ifndef RETROWAVE_UTILS_INCLUDED
#define RETROWAVE_UTILS_INCLUDED

// expected results as f(t):
// (-inf; start)        => 0
// (start; fullMin)     => lerp 0 -> 1
// (fullMin; fullMax)   => 1
// (fullMax; end)       => lerp 1 -> 0;
// (end; +inf)          => 0;
half sampleGradientPoint(half start, half fullMin, half fullMax, half end, half t)
{
    return
    lerp(0, 1, ((t - start) / (fullMin - start))) * (t >= start && t < fullMin)
        + 1 * (t >= fullMin && t <= fullMax)
        + lerp(1, 0, ((t - fullMax) / (end - fullMax))) * (t > fullMax && t <= end);
}

half3 RGBToHSV(half3 c)
{
    half4 K = half4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    half4 p = lerp(half4(c.bg, K.wz), half4(c.gb, K.xy), step(c.b, c.g));
    half4 q = lerp(half4(p.xyw, c.r), half4(c.r, p.yzx), step(p.x, c.r));
    half d = q.x - min(q.w, q.y);
    half e = 1.0e-10;
    return half3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

half3 HSVToRGB(half3 c)
{
    half4 K = half4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    half3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
}

#endif