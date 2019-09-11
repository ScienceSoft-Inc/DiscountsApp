import { Pipe, PipeTransform } from '@angular/core';
// code from https://stackoverflow.com/questions/37591076/how-to-convert-seconds-to-time-string-in-angular2

@Pipe({
  name: 'secondsToTime'
})
export class SecondsToTimePipe implements PipeTransform {
  timesEng = {
    h: 3600,
    m: 60,
    s: 1
  };
  timesRus = {
    ч: 3600,
    м: 60,
    с: 1
  };

  transform(seconds: number, isEnglish: boolean): string {
    let time_string = '';
    const times = isEnglish ? this.timesEng : this.timesRus;
    for (const key of Object.keys(times)) {
      const tResult = seconds / times[key];
      if (Math.floor(tResult) > 0 || (Math.floor(tResult) === 0 )) {
        time_string += Math.floor(tResult).toLocaleString('en-US', {minimumIntegerDigits: 2} )
          + key.toString() + (key === 's' || key === 'с' ? '' : ':');
        seconds = seconds - times[key] * Math.floor(tResult);
      }
    }
    return time_string;
  }
}
