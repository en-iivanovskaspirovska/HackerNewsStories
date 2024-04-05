import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { Story } from './models/story';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('AppComponent', () => {
  let httpTestingController:HttpTestingController;
  let app: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent, HttpClientTestingModule],
      declarations: [AppComponent],
      providers: [{provide: 'BASE_URL', useValue: 'https://localhost:7047/stories'}]
    }).compileComponents();
    fixture = TestBed.createComponent(AppComponent);
    app = fixture.componentInstance;
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'HackerNewsApiConsumer' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('HackerNewsApiConsumer');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hacker News Stories');
  });

  it('should show hacker news stories on load', () => {
    const stories: Story[] = [
      {title: 'First Story', url: 'http://storyurl1.com/1'},
      {title: 'Second Story', url: 'http://storyurl1.com/2'},
    ];

    app.ngOnInit();
    
    const request =  httpTestingController.expectOne('https://localhost:7047/stories');
    request.flush(stories);

    expect(app.stories).toEqual(stories); 
  })
});
