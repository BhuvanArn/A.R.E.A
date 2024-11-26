import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { toSignal } from '@angular/core/rxjs-interop'; // Import toSignal for reactive handling

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'angular_poc';
  userInput: string = '';
  menuOpen: boolean = false;
  responseData: string = '';

  constructor(private http: HttpClient) {}

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  async onButtonClick(): Promise<void> {
    const url = this.userInput;
    if (!url) {
      alert('Please enter a URL.');
      return;
    }

    alert("URL used : " + url);
    console.log("URL used : ", url);
    // try {
      // Use for-await-of to handle the observable
      // const response = await toSignal(this.http.get(url))();
      // const response = this.http.get(url);
      // console.log('GET response:', response);
      // alert('Success! Check the console for the response.' + response);
      const response = this.http.get(url);
      response.subscribe(
        (data) => {
          // alert('GET response: ' + data);
          this.responseData = JSON.stringify(data, null, 2);
          console.log('GET response: ', data);
          console.log(data);
        },
        (error) => {
          console.error('Error occurred:', error);
          this.responseData = 'Error: Unable to fetch data. Check the console for details.';
        }
      );

    // } catch (error) {
    //   console.error('GET error:', error);
    //   alert('Failed to fetch data. Check the console for details.' + error);
    // }
  }
}
