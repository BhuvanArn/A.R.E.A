import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { AppComponent } from './app/app.component';

bootstrapApplication(AppComponent, {
  providers: [provideHttpClient()] // Add HttpClient provider
}).catch((err) => console.error(err));

// bootstrapApplication(AppComponent, {
//   providers: [provideHttpClient(), ...appConfig.providers], // Add HttpClient provider
// }).catch((err) => console.error(err));

// bootstrapApplication(AppComponent, appConfig)
//   .catch((err) => console.error(err));
