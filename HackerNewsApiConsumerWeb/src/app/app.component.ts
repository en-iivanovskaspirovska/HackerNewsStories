import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Story } from './models/story'
import { NgFor, NgIf } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
 
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule, NgFor, NgIf, NgxPaginationModule],
  templateUrl: './app.component.html',
  styleUrl: '../../node_modules/bootstrap/dist/css/bootstrap.min.css'
})
export class AppComponent implements OnInit{
  title = 'HackerNewsApiConsumer';
  httpClient = inject(HttpClient);
  stories: Story[] = [];
  page: any;
  filteredStories: Story[] = [];
  fetchStories():void{
    this.httpClient.get<Story[]>('https://localhost:7047/stories').subscribe((data)=>{
    this.stories = data;
    this.filteredStories = data;
    console.log(this.stories);
    });
  }
  ngOnInit():void{
  this.fetchStories();
  }

  filterResults(text:string){
    if(!text){
      this.filteredStories=this.stories;
    }
    else
    {
      this.filteredStories=this.stories.filter(story => story.title.toLowerCase().includes(text.toLowerCase()));
    }
  }
}
