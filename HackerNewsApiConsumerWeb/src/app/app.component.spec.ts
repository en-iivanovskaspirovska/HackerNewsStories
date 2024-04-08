import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { Story } from './models/story';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
 
describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
 
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    }).compileComponents();
  });
 
  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });
 
  afterEach(() => {
    httpTestingController.verify();
  });
 
  it('should create the app', () => {
    expect(component).toBeTruthy();
  });
 
  it(`should have the 'HackerNewsApiConsumer' title`, () => {
    expect(component.title).toEqual('HackerNewsApiConsumer');
  });
 
  it('should render title', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain(
      'Hacker News Stories'
    );
  });
 
  it('should show hacker news stories on load', () => {
    const stories: Story[] = [
      { title: 'Story 1', url: 'http://example.com/story1' },
      { title: 'Story 2', url: 'http://example.com/story2' },
    ];
 
    spyOn(httpClient, 'get').and.returnValue(of(stories));
    component.ngOnInit();
 
    fixture.whenStable().then(() => {
      expect(component.stories.length).toBe(2);
      expect(component.stories).toEqual(stories);
    });
  });
 
  it('should filter stories based on input text', () => {
    const stories: Story[] = [
      { title: 'Story 1', url: 'http://example.com/story1' },
      { title: 'Story 2', url: 'http://example.com/story2' },
    ];
 
    component.stories = stories;
 
    component.filterResults('Story 1');
 
    expect(component.filteredStories.length).toBe(1);
    expect(component.filteredStories[0].title).toBe('Story 1');
  });
});