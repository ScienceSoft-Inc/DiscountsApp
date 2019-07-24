export class ColorHelper {
  // The idea for getting color's text by background hex was borrowed here
  // https://stackoverflow.com/questions/1855884/determine-font-color-based-on-background-color/37313492

  public static GetLuminanceByHex(colourInHex: string): number {
    const hex = colourInHex.replace(/#/, '');
    const r = parseInt(hex.substr(0, 2), 16);
    const g = parseInt(hex.substr(2, 2), 16);
    const b = parseInt(hex.substr(4, 2), 16);
    const luminance = ( 0.299 * r + 0.587 * g + 0.114 * b) / 255;

    return luminance;
  }

  public static GetColorByBackHex(colourInHex: string): string {
    const luminance = this.GetLuminanceByHex(colourInHex);
    return luminance > 0.5 ? '#000000' : '#ffffff';
  }

  public static IsBlackColorByBackHex(colourInHex: string): boolean {
    const luminance = this.GetLuminanceByHex(colourInHex);
    return luminance > 0.5;
  }
}
